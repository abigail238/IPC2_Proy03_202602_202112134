using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores;

[ApiController]
[Route("api/sistema")]
public class SistemaController : ControllerBase
{
    private readonly SistemaService _servicio;

    public SistemaController(
        SistemaService servicio)
    {
        _servicio = servicio;
    }

    // Endpoint:
    //
    // DELETE /api/sistema/inicializar
    [HttpDelete("inicializar")]
    public IActionResult Inicializar()
    {
        // Le pedimos al servicio que vacíe
        // los archivos de datos.
        _servicio.Inicializar();

        return Ok(new
        {
            mensaje =
                "El sistema fue inicializado correctamente."
        });
    }
}