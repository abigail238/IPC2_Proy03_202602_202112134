using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores;

[ApiController]
[Route("api/autorizaciones")]
public class AutorizacionesController : ControllerBase
{
    private readonly AutorizacionesService _servicio;

    public AutorizacionesController(AutorizacionesService servicio)
    {
        _servicio = servicio;
    }

    [HttpPost]
    public IActionResult Procesar([FromBody] List<SolicitudAutorizacion>? solicitudes)
    {
        if (solicitudes is null || solicitudes.Count == 0 || solicitudes.Any(s => s is null))
        {
            return BadRequest(new { mensaje = "Se requiere una lista no vacia de solicitudes sin elementos nulos" });
        }

        return Ok(_servicio.Procesar(solicitudes));
    }
}
