using IPC2_Proy03.Frontend.Modelos;
using IPC2_Proy03.Frontend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class ResumenIvaModel : PageModel
{
    private readonly ApiService _apiService;

    public ResumenIvaModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public DateTime Fecha { get; set; } = DateTime.Today;

    public List<ResumenIvaNit> Resultado { get; set; } = [];
    public string? Error { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            Resultado = await _apiService.GetJsonAsync<List<ResumenIvaNit>>(
                $"/api/consultas/iva?fecha={Fecha:yyyy-MM-dd}") ?? [];
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return Page();
    }
}
