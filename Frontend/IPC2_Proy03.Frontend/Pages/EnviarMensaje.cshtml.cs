using IPC2_Proy03.Frontend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class EnviarMensajeModel : PageModel
{
    private readonly ApiService _apiService;


    // ASP.NET Core nos proporciona ApiService
    // automáticamente mediante inyección de dependencias.
    public EnviarMensajeModel( ApiService apiService)
    {
        _apiService = apiService;
    }


    // El archivo seleccionado desde la página HTML.
    [BindProperty]
    public IFormFile? ArchivoJson { get; set; }


    // Indica qué clase de archivo estamos enviando:
    //
    // RTU
    // AUTORIZACIONES
    [BindProperty]
    public string TipoMensaje { get; set; }
        = "RTU";


    // Aquí mostraremos la respuesta de la API.
    public string? Resultado { get; set; }


    // Aquí mostraremos cualquier error.
    public string? Error { get; set; }


    public void OnGet()
    {
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (TipoMensaje != "RTU" && TipoMensaje != "AUTORIZACIONES")
        {
            Error = "Seleccione un tipo de mensaje válido.";
            return Page();
        }
        
        // VALIDAR ARCHIVO
        

        if (ArchivoJson == null ||
            ArchivoJson.Length == 0)
        {
            Error = "Debe seleccionar un archivo JSON.";

            return Page();
        }


        string extension =Path.GetExtension( ArchivoJson.FileName);


        // Solo permitimos archivos .json.
        if (!extension.Equals( ".json", StringComparison.OrdinalIgnoreCase))
        {
            Error =
                "El archivo seleccionado debe ser JSON.";

            return Page();
        }


        try
        {
           
            // LEER ARCHIVO
            
            using StreamReader lector =
                new StreamReader(
                    ArchivoJson.OpenReadStream()
                );


            string json =
                await lector.ReadToEndAsync();


            // DETERMINAR ENDPOINT
            

            string endpoint;


            if (TipoMensaje == "RTU")
            {
                endpoint = "/api/rtu";
            }
            else
            {
                endpoint = "/api/autorizaciones";
            }


            // ENVIAR AL BACKEND
            

            Resultado = await _apiService.PostJsonAsync( endpoint,json);
        }
        catch (Exception ex)
        {
            // Si la API devuelve error,
            // mostramos el mensaje en pantalla.
            Error = ex.Message;
        }


        return Page();
    }
}
