using System.Text.Json;
using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Utils;

namespace IPC2_Proy03.Api.Servicios;

public class AutorizacionService
{
    private readonly NitService _nitService;

    private readonly string _rutaContribuyentes;
    private readonly string _rutaAutorizaciones;

    //ASP.NET nos proporciona automaticamente
    //NitService y IWebHostEnvironment en el constructor de la clase
    //gracias a la inyeccion de dependencias

    public AutorizacionService(NitService nitService, IWebHostEnvironment environment)
    {
        _nitService = nitService;
        _rutaContribuyentes = Path.Combine(environment.ContentRootPath, "Datos", "Contribuyentes.json");

        _rutaAutorizaciones = Path.Combine(environment.ContentRootPath, "Datos", "Autorizaciones.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_rutaAutorizaciones)!);

        //Si todavia no existe el archivo
        //lo incializamos con un arreglo vacio
        if (!File.Exists(_rutaAutorizaciones))
        {
            File.WriteAllText(_rutaAutorizaciones, "[]");
        }


    }
    //Metodo principal

    public ResultadoAutorizaciones Procesar(List<SolicitudAutorizacion> solicitudes)
    {
        //cargamos la informacion persistida
        List<Contribuyente> contribuyentes = CargarContribuyentes();


        List<RegistroAutorizaciones> registros = CargarRegistro();

        ResultadoAutorizaciones respuesta = new ResultadoAutorizaciones();

        foreach (SolicitudAutorizacion solicitud in solicitudes)
        {
            solicitud.NIT_EMISOR = NitService.Normalizar(solicitud.NIT_EMISOR);
            solicitud.NIT_RECEPTOR = NitService.Normalizar(solicitud.NIT_RECEPTOR);
            RegistroAutorizaciones registro = new RegistroAutorizaciones
            {
                Referencia = solicitud.REFERENCIA,
                NitEmisor = solicitud.NIT_EMISOR,
                NitReceptor = solicitud.NIT_RECEPTOR,
                Valor = solicitud.VALOR,
                Iva = solicitud.IVA,
                Total = solicitud.TOTAL
            };

            //VALIDAR FECHA

            DateTime? fecha = FechaParser.ExtraerFecha(solicitud.TIEMPO);

            if (!fecha.HasValue)
            {
                registro.Errores.Add("FECHA_INVALIDA");

            }
            else
            {
                registro.Fecha = fecha.Value;
            }

            // 2. validar Referencia

            if (string.IsNullOrWhiteSpace(solicitud.REFERENCIA))
            {
                registro.Errores.Add("REFERENCIA_INVALIDA");
            }
            else if (solicitud.REFERENCIA.Length > 40)
            {
                registro.Errores.Add("REFERENCIA_INVALIDA");

            }

            //Buscamos la referencia en todo el sistema
            bool referenciaExistente = registros.Any(r => r.Referencia == solicitud.REFERENCIA);

            if (referenciaExistente)
            {
                registro.Errores.Add("REFERENCIA_DUPLICADA");

            }

            //3. VALIDAR NIT EMISOR

            bool nitEmisorValido = _nitService.EsNitValido(solicitud.NIT_EMISOR);

            if (!nitEmisorValido)
            {
                registro.Errores.Add("NIT_EMISOR_INVALIDO");

            }
            else
            {
                //si matematicamente es valido
                //ahora comprobamos que exista en el RTU

                bool emisorExistente = contribuyentes.Any(c => NitService.Normalizar(c.Nit) == solicitud.NIT_EMISOR);


                if (!emisorExistente)
                {
                    registro.Errores.Add("NIT_EMISOR_INEXISTENTE");

                    respuesta.resultadoAutorizaciones.cantidadAutorizacionesRechazadasPorNitEmisorInexistente++;

                }
            }

            //4. Validar NIT receptor

            bool nitReceptorValido = _nitService.EsNitValido(solicitud.NIT_RECEPTOR);

            if (!nitReceptorValido)
            {
                registro.Errores.Add("NIT_RECEPTOR_INVALIDO");
            }

            else
            {
                bool receptorExiste = contribuyentes.Any(c => NitService.Normalizar(c.Nit) == solicitud.NIT_RECEPTOR);

                if (!receptorExiste)
                {
                    registro.Errores.Add("NIT_RECEPTOR_INEXISTENTE");

                    respuesta.resultadoAutorizaciones.cantidadAutorizacionesRechazadasPorNitReceptorInexistente++;


                }


            }

            //Validar valor

            if (solicitud.VALOR <= 0)
            {
                registro.Errores.Add("VALOR_INVALIDO");
            }

            // validar iva
            decimal ivaEsperado =
                    Math.Round(
                        solicitud.VALOR * 0.12m,
                        2,
                        MidpointRounding.AwayFromZero
                    );

            if (solicitud.IVA != ivaEsperado)
            {
                registro.Errores.Add("IVA_MAL_CALCULADO");
            }

            //validar total

            decimal totalEsperado =
                    solicitud.VALOR + solicitud.IVA;

            if (solicitud.TOTAL != totalEsperado)
            {
                registro.Errores.Add("TOTAL_MAL_CALCULADO");
            }

            //si hay errores

            if (registro.Errores.Count > 0)
            {
                registro.Aprobada = false;

                // Aunque esté rechazada, guardamos la solicitud.
                //
                // Esto servirá posteriormente para reportes.
                registros.Add(registro);

                continue;
            }

            //9. generar codigo de aprobacion

            long codigo =
                    GenerarCodigoAprobacion(
                        fecha!.Value,
                        registros
                    );


            registro.CodigoAprobacion = codigo;
            registro.Aprobada = true;


            registros.Add(registro);


            respuesta.resultadoAutorizaciones
                .cantidadAutorizacionesAprobadas++;


            respuesta.resultadoAutorizaciones
                .detalleAprobaciones
                .Add(
                    new DetalleAprobacion
                    {
                        referencia = solicitud.REFERENCIA,
                        codigoAprobacion = codigo
                    }
                );

        }

        // Guardamos todas las solicitudes procesadas.
        GuardarRegistros(registros);


        return respuesta;

    }



    private List<Contribuyente> CargarContribuyentes()
    {
        if (!File.Exists(_rutaContribuyentes))
        {
            return [];
        }

        string json = File.ReadAllText(_rutaContribuyentes);
        return JsonSerializer.Deserialize<List<Contribuyente>>(json) ?? [];
    }

    private List<RegistroAutorizaciones> CargarRegistro()
    {
        string json = File.ReadAllText(_rutaAutorizaciones);
        return JsonSerializer.Deserialize<List<RegistroAutorizaciones>>(json) ?? [];
    }

    private void GuardarRegistros(List<RegistroAutorizaciones> registros)
    {
        string json = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_rutaAutorizaciones, json);
    }

    private static long GenerarCodigoAprobacion(DateTime fecha, List<RegistroAutorizaciones> registros)
    {
        long prefijo = long.Parse(fecha.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)) * 100000000L;
        long ultimoCodigo = registros
            .Where(r => r.Aprobada && r.Fecha?.Date == fecha.Date && r.CodigoAprobacion.HasValue)
            .Select(r => r.CodigoAprobacion!.Value)
            .DefaultIfEmpty(prefijo)
            .Max();

        return ultimoCodigo + 1;
    }


}
