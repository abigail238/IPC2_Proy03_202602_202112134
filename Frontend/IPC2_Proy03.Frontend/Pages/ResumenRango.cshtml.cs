using IPC2_Proy03.Frontend.Modelos;
using IPC2_Proy03.Frontend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class ResumenRangoModel : PageModel
{
    private readonly ApiService _apiService;

    public ResumenRangoModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public DateTime FechaInicio { get; set; } = DateTime.Today.AddMonths(-1);

    [BindProperty]
    public DateTime FechaFin { get; set; } = DateTime.Today;

    [BindProperty]
    public bool IncluirIva { get; set; } = true;

    public List<ResumenRangoFecha> Resultado { get; set; } = [];
    public string? Error { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (FechaInicio.Date > FechaFin.Date)
        {
            Error = "La fecha inicial no puede ser mayor que la fecha final.";
            return Page();
        }

        try
        {
            Resultado = await _apiService.GetJsonAsync<List<ResumenRangoFecha>>(
                $"/api/consultas/rango?fechaInicio={FechaInicio:yyyy-MM-dd}" +
                $"&fechaFin={FechaFin:yyyy-MM-dd}&incluirIva={IncluirIva}") ?? [];
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }

        return Page();
    }
}
