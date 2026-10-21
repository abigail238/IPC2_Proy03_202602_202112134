using IPC2_Proy03.Api.Modelos;
using IPC2_Proy03.Api.Servicios;
using Microsoft.AspNetCore.Mvc;


namespace IPC2_Proy03.Api.Controladores;

//indicamos que esta clase sera un controlador de API
[ApiController]

//definimos la ruta base
//este controladores respondera en : /api/rtu

[Route("api/rtu")]

public class RtuController : ControllerBase
{
    //guardamos una referencia al servicio encargado 
    //de procesar la informacion de RTU
    private readonly RtuService _rtuService;

    //ASP.NET core nos entregara automaticamente una instancia de RtuService
    //gracisa a la inyeccion de dependencias
    public RtuController (RtuService rtuService)
    {
        _rtuService = rtuService;
    }

    //este metodo respodera a:
    //POST /api/rtu
    //recibira un arreglo JSON de contribuyentes 

    [HttpPost]
    public IActionResult ActualizarRtu([FromBody] List<Contribuyente>? contribuyentes)
    {
        Console.WriteLine("Estamos dentro del metodo post");

        //verificamos que realmente se haya enviado informacion 
        if(contribuyentes == null || contribuyentes.Count == 0 || contribuyentes.Any(c => c is null))
        {
            return BadRequest(new { mensaje = "No se recibieron contribuyentes para procesar" });
        }

        Console.WriteLine("estos son los contribuyentes que se recibieron: "+contribuyentes.Count);

        //mandamos la lista al servicio 
        //el servicion se encgara de:
        //validar nit, crear nuevos, actualizar existentes y contar los resultados

         ResultadosRtu resultado = _rtuService.ProcesarRtu(contribuyentes);


        //devolcemos HTTP 200 junto con el resultado 
        return Ok(resultado);
    }


}
