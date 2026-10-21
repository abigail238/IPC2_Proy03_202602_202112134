using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores;

[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly ConsultaService _servicio;


    public ConsultasController(
        ConsultaService servicio)
    {
        _servicio = servicio;
    }


    
    // CONSULTAR CONTRIBUYENTE
    
    
    // GET
    // /api/consultas/contribuyente
    

    [HttpGet("contribuyente")]
    public IActionResult ConsultarContribuyente(
        [FromQuery] string nit,
        [FromQuery] string tipo,
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            return BadRequest(
                new
                {
                    mensaje = "Debe indicar un NIT."
                }
            );
        }


        if (tipo != "emitidas" &&
            tipo != "recibidas")
        {
            return BadRequest(
                new
                {
                    mensaje =
                        "El tipo debe ser emitidas o recibidas."
                }
            );
        }


        if (fechaInicio.Date > fechaFin.Date)
        {
            return BadRequest(
                new
                {
                    mensaje =
                        "La fecha inicial no puede ser mayor que la fecha final."
                }
            );
        }


        ConsultaContribuyenteResultado resultado =
            _servicio.ConsultarContribuyente(
                nit,
                tipo,
                fechaInicio,
                fechaFin
            );


        return Ok(resultado);
    }


    
    // RESUMEN IVA POR FECHA
   
    // GET /api/consultas/iva?fecha=2026-10-20

    [HttpGet("iva")]
    public IActionResult ObtenerResumenIva(
        [FromQuery] DateTime fecha)
    {
        List<ResumenIvaNit> resultado =
            _servicio.ObtenerResumenIva(fecha);

        return Ok(resultado);
    }


    
    // RESUMEN POR RANGO
    
    
    // Ejemplo:
    
    // GET
    // /api/consultas/rango
    

    [HttpGet("rango")]
    public IActionResult ObtenerResumenRango(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin,
        [FromQuery] bool incluirIva = true)
    {
        if (fechaInicio.Date > fechaFin.Date)
        {
            return BadRequest(
                new
                {
                    mensaje =
                        "La fecha inicial no puede ser mayor que la fecha final."
                }
            );
        }


        List<ResumenRangoFecha> resultado =
            _servicio.ObtenerResumenRango(
                fechaInicio,
                fechaFin,
                incluirIva
            );


        return Ok(resultado);
    }
}