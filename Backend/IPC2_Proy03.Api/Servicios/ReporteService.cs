using System.Text.Json;
using IPC2_Proy03.Api.Modelos;

namespace IPC2_Proy03.Api.Servicios;

// Este servicio genera los reportes estadísticos
// a partir de las solicitudes almacenadas en Autorizaciones.json.
public class ReporteService
{
    private readonly string _rutaAutorizaciones;

    public ReporteService(IWebHostEnvironment environment)
    {
        // Construimos la ruta completa del archivo.
        _rutaAutorizaciones = Path.Combine(
            environment.ContentRootPath,
            "Datos",
            "Autorizaciones.json"
        );

        // Nos aseguramos de que la carpeta Datos exista.
        Directory.CreateDirectory(
            Path.GetDirectoryName(_rutaAutorizaciones)!
        );

        // Si el archivo todavía no existe,
        // lo inicializamos como una lista vacía.
        if (!File.Exists(_rutaAutorizaciones))
        {
            File.WriteAllText(
                _rutaAutorizaciones,
                "[]"
            );
        }
    }

    // Lee todas las solicitudes que ya fueron procesadas.
    private List<RegistroAutorizaciones> CargarRegistros()
    {
        string json =
            File.ReadAllText(_rutaAutorizaciones);

        return JsonSerializer
                   .Deserialize<List<RegistroAutorizaciones>>(json)
               ?? [];
    }

    // Genera el reporte para un rango de fechas.
    public List<ReporteFecha> GenerarReporte(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        List<RegistroAutorizaciones> registros =
            CargarRegistros();

        // Nos quedamos únicamente con registros
        // que tengan una fecha válida y estén dentro del rango.
        List<RegistroAutorizaciones> registrosFiltrados =
            registros
                .Where(r =>
                    r.Fecha.HasValue &&
                    r.Fecha.Value.Date >= fechaInicio.Date &&
                    r.Fecha.Value.Date <= fechaFin.Date
                )
                .ToList();

        // Agrupamos las solicitudes por día.
        var grupos =
            registrosFiltrados
                .GroupBy(r => r.Fecha!.Value.Date)
                .OrderBy(g => g.Key);

        List<ReporteFecha> resultado = [];

        foreach (var grupo in grupos)
        {
            ReporteFecha reporte =
                new ReporteFecha
                {
                    Fecha = grupo.Key,

                    // Cantidad total de solicitudes recibidas.
                    CantidadTotalFacturasRecibidas =
                        grupo.Count(),

                    // Contamos cada tipo de error.
                    NitEmisorInvalido =
                        grupo.Count(r =>
                            r.Errores.Contains("NIT_EMISOR_INVALIDO")
                        ),

                    NitReceptorInvalido =
                        grupo.Count(r =>
                            r.Errores.Contains("NIT_RECEPTOR_INVALIDO")
                        ),

                    NitEmisorInexistente =
                        grupo.Count(r =>
                            r.Errores.Contains("NIT_EMISOR_INEXISTENTE")
                        ),

                    NitReceptorInexistente =
                        grupo.Count(r =>
                            r.Errores.Contains("NIT_RECEPTOR_INEXISTENTE")
                        ),

                    IvaMalCalculado =
                        grupo.Count(r =>
                            r.Errores.Contains("IVA_MAL_CALCULADO")
                        ),

                    TotalMalCalculado =
                        grupo.Count(r =>
                            r.Errores.Contains("TOTAL_MAL_CALCULADO")
                        ),

                    ReferenciaDuplicada =
                        grupo.Count(r =>
                            r.Errores.Contains("REFERENCIA_DUPLICADA")
                        ),

                    // Las facturas aprobadas son las que no tuvieron errores.
                    CantidadFacturasSinErrores =
                        grupo.Count(r => r.Aprobada),

                    // Contamos emisores diferentes.
                    CantidadEmisores =
                        grupo
                            .Select(r => r.NitEmisor)
                            .Where(nit =>
                                !string.IsNullOrWhiteSpace(nit)
                            )
                            .Distinct()
                            .Count(),

                    // Contamos receptores diferentes.
                    CantidadReceptores =
                        grupo
                            .Select(r => r.NitReceptor)
                            .Where(nit =>
                                !string.IsNullOrWhiteSpace(nit)
                            )
                            .Distinct()
                            .Count()
                };

            resultado.Add(reporte);
        }

        return resultado;
    }
}