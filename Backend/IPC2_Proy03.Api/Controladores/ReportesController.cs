using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly ReporteService _reporteService;
    private readonly PdfService _pdfService;


    public ReportesController(
        ReporteService reporteService,
        PdfService pdfService)
    {
        _reporteService = reporteService;
        _pdfService = pdfService;
    }


    // ---------------------------------------------------------
    // REPORTE JSON
    // ---------------------------------------------------------

    [HttpGet]
    public IActionResult ObtenerReporte(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        if (fechaInicio.Date > fechaFin.Date)
        {
            return BadRequest(new
            {
                mensaje =
                    "La fecha inicial no puede ser mayor que la fecha final."
            });
        }


        List<ReporteFecha> resultado =
            _reporteService.GenerarReporte(
                fechaInicio,
                fechaFin
            );


        return Ok(resultado);
    }


    // ---------------------------------------------------------
    // REPORTE PDF
    // ---------------------------------------------------------
    //
    // GET /api/reportes/pdf
    // ?fechaInicio=2026-10-01
    // &fechaFin=2026-10-31

    [HttpGet("pdf")]
    public IActionResult GenerarPdf(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        if (fechaInicio.Date > fechaFin.Date)
        {
            return BadRequest(
                "La fecha inicial no puede ser mayor que la fecha final."
            );
        }


        List<ReporteFecha> reportes =
            _reporteService.GenerarReporte(
                fechaInicio,
                fechaFin
            );


        byte[] pdf =
            _pdfService.GenerarReporte(
                reportes,
                fechaInicio,
                fechaFin
            );


        string nombreArchivo =
            $"Reporte_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.pdf";


        // Retornamos un archivo PDF al cliente.
        return File(
            pdf,
            "application/pdf",
            nombreArchivo
        );
    }
}