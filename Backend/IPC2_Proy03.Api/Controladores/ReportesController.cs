using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly ReporteService _servicio;

    // ASP.NET Core inyecta automáticamente
    // el ReporteService registrado en Program.cs.
    public ReportesController(
        ReporteService servicio)
    {
        _servicio = servicio;
    }

    // Endpoint:
    //
    // GET /api/reportes
    //
    // Ejemplo:
    //
    // /api/reportes?fechaInicio=2026-10-01&fechaFin=2026-10-31
    [HttpGet]
    public IActionResult ObtenerReporte(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        // Validamos que el rango sea correcto.
        if (fechaInicio.Date > fechaFin.Date)
        {
            return BadRequest(new
            {
                mensaje =
                    "La fecha inicial no puede ser mayor que la fecha final."
            });
        }

        List<ReporteFecha> resultado =
            _servicio.GenerarReporte(
                fechaInicio,
                fechaFin
            );

        return Ok(resultado);
    }
}