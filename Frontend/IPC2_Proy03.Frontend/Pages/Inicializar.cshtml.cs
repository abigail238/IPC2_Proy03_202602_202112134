using IPC2_Proy03.Frontend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class InicializarModel : PageModel
{
    private readonly ApiService _apiService;

    public InicializarModel(
        ApiService apiService)
    {
        _apiService = apiService;
    }

    // Mensaje mostrado cuando la operación es exitosa.
    public string? Resultado { get; set; }

    // Mensaje mostrado si ocurre un error.
    public string? Error { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            // Llamamos al endpoint del backend
            // encargado de borrar los datos del sistema.
            Resultado =
                await _apiService.DeleteAsync(
                    "/api/sistema/inicializar"
                );
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return Page();
    }
}