using System.Text.Json;
using IPC2_Proy03.Api.Modelos;

namespace IPC2_Proy03.Api.Servicios;

// Servicio encargado de realizar consultas y
// generar resúmenes a partir de las facturas procesadas.
public class ConsultaService
{
    private readonly string _rutaAutorizaciones;

    public ConsultaService(IWebHostEnvironment environment)
    {
        // Construimos la ruta donde se encuentran
        // las autorizaciones almacenadas.
        _rutaAutorizaciones = Path.Combine(
            environment.ContentRootPath,
            "Datos",
            "Autorizaciones.json"
        );

        // Si todavía no existe el archivo,
        // lo inicializamos vacío.
        if (!File.Exists(_rutaAutorizaciones))
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(_rutaAutorizaciones)!
            );

            File.WriteAllText(
                _rutaAutorizaciones,
                "[]"
            );
        }
    }


   
    // CARGAR AUTORIZACIONES
    

    private List<RegistroAutorizaciones> CargarRegistros()
    {
        lock (SistemaService.BloqueoDatos)
        {
            string json =
                File.ReadAllText(_rutaAutorizaciones);

            return JsonSerializer
                       .Deserialize<List<RegistroAutorizaciones>>(json)
                   ?? [];
        }
    }


    
    // CONSULTAR CONTRIBUYENTE
    

    public ConsultaContribuyenteResultado ConsultarContribuyente(
        string nit,
        string tipo,
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        // Normalizamos el tipo para evitar problemas
        // con mayúsculas/minúsculas.
        tipo = tipo.Trim().ToLowerInvariant();
        nit = NitService.Normalizar(nit);

        List<RegistroAutorizaciones> registros =
            CargarRegistros();

        // Solamente utilizamos facturas aprobadas,
        // porque son las facturas autorizadas.
        IEnumerable<RegistroAutorizaciones> consulta =
            registros.Where(r =>
                r.Aprobada &&
                r.Fecha.HasValue &&
                r.Fecha.Value.Date >= fechaInicio.Date &&
                r.Fecha.Value.Date <= fechaFin.Date
            );

        // Si el usuario solicita "emitidas",
        // buscamos donde el NIT sea el emisor.
        if (tipo == "emitidas")
        {
            consulta = consulta.Where(
                r => NitService.Normalizar(r.NitEmisor) == nit
            );
        }
        // Si solicita "recibidas",
        // buscamos donde el NIT sea receptor.
        else if (tipo == "recibidas")
        {
            consulta = consulta.Where(
                r => NitService.Normalizar(r.NitReceptor) == nit
            );
        }
        else
        {
            // Si llega un tipo incorrecto,
            // devolvemos un resultado vacío.
            return new ConsultaContribuyenteResultado
            {
                Nit = nit,
                Tipo = tipo
            };
        }


        // Agrupamos las facturas por fecha.
        List<ResumenFechaContribuyente> detalle =
            consulta
                .GroupBy(r => r.Fecha!.Value.Date)
                .OrderBy(g => g.Key)
                .Select(g =>
                    new ResumenFechaContribuyente
                    {
                        Fecha = g.Key,

                        CantidadFacturas = g.Count(),

                        MontoTotal = g.Sum(
                            r => r.Total
                        )
                    }
                )
                .ToList();


        // Calculamos el total de todo el período.
        decimal totalFacturado =
            detalle.Sum(d => d.MontoTotal);


        return new ConsultaContribuyenteResultado
        {
            Nit = nit,
            Tipo = tipo,
            Detalle = detalle,
            TotalFacturado = totalFacturado
        };
    }


    
    // RESUMEN IVA POR FECHA
    

    public List<ResumenIvaNit> ObtenerResumenIva(
        DateTime fecha)
    {
        List<RegistroAutorizaciones> registros =
            CargarRegistros();


        // Nos quedamos únicamente con facturas aprobadas
        // correspondientes al día solicitado.
        List<RegistroAutorizaciones> facturas =
            registros
                .Where(r =>
                    r.Aprobada &&
                    r.Fecha.HasValue &&
                    r.Fecha.Value.Date == fecha.Date
                )
                .ToList();


        // Obtenemos todos los NIT que participaron
        // como emisores o receptores.
        List<string> nits =
            facturas
                .SelectMany(r =>
                    new[]
                    {
                        r.NitEmisor,
                        r.NitReceptor
                    }
                )
                .Distinct()
                .ToList();


        List<ResumenIvaNit> resultado = [];


        foreach (string nit in nits)
        {
            // IVA generado por facturas donde
            // este contribuyente fue emisor.
            decimal ivaEmitido =
                facturas
                    .Where(r =>
                        r.NitEmisor == nit
                    )
                    .Sum(r => r.Iva);


            // IVA correspondiente a facturas donde
            // este contribuyente fue receptor.
            decimal ivaRecibido =
                facturas
                    .Where(r =>
                        r.NitReceptor == nit
                    )
                    .Sum(r => r.Iva);


            resultado.Add(
                new ResumenIvaNit
                {
                    Nit = nit,
                    IvaEmitido = ivaEmitido,
                    IvaRecibido = ivaRecibido
                }
            );
        }


        return resultado;
    }



    // RESUMEN POR RANGO
    

    public List<ResumenRangoFecha> ObtenerResumenRango(
        DateTime fechaInicio,
        DateTime fechaFin,
        bool incluirIva)
    {
        List<RegistroAutorizaciones> registros =
            CargarRegistros();


        IEnumerable<RegistroAutorizaciones> facturas =
            registros.Where(r =>
                r.Aprobada &&
                r.Fecha.HasValue &&
                r.Fecha.Value.Date >= fechaInicio.Date &&
                r.Fecha.Value.Date <= fechaFin.Date
            );


        // Agrupamos todas las facturas por fecha.
        List<ResumenRangoFecha> resultado =
            facturas
                .GroupBy(r =>
                    r.Fecha!.Value.Date
                )
                .OrderBy(g => g.Key)
                .Select(g =>
                    new ResumenRangoFecha
                    {
                        Fecha = g.Key,

                        // Si incluirIva = true usamos TOTAL.
                        //
                        // Si incluirIva = false usamos VALOR,
                        // que representa el monto sin IVA.
                        ValorAutorizado =
                            incluirIva
                                ? g.Sum(r => r.Total)
                                : g.Sum(r => r.Valor)
                    }
                )
                .ToList();


        return resultado;
    }
}
