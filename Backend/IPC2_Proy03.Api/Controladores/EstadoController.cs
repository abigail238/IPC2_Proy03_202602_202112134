using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Api.Controladores
{
    //indicar que esta clase funcionara como un controlador API
    [ApiController]

    //indicar la ruta de acceso a este controlador 
    [Route("api/estado")]
    public class EstadoController : ControllerBase
    {
        //indicar que este metodo respondera a una solicitud HTTP 
        [HttpGet]

        public IActionResult ObtenerEstado()
        {
            //retornar un mensaje de respuesta con el estado del servidor 

            var respuesta = new
            {
                mensaje = "API funcionando correctamente"
            };

            return Ok(respuesta);
        }



    }

}
