using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using IPC2_Proy03.Frontend.Servicios;
using IPC2_Proy03.Frontend.Modelos;

namespace IPC2_Proy03.Frontend.Pages;

public class ConsultarContribuyenteModel : PageModel
{
    private readonly ApiService _apiService;

    public ConsultarContribuyenteModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public string Nit { get; set; } = string.Empty;
    [BindProperty]
    public string Tipo { get; set; } = "emitidas";
    [BindProperty]
    public DateTime FechaInicio { get; set; } = DateTime.Today.AddMonths(-1);
    [BindProperty]
    public DateTime FechaFin { get; set; } = DateTime.Today;

    public ConsultaContribuyenteResultado? Resultado { get; set; }
    public string? Error { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (string.IsNullOrWhiteSpace(Nit) || (Tipo != "emitidas" && Tipo != "recibidas"))
        {
            Error = "Indique un NIT y seleccione emitidas o recibidas.";
            return Page();
        }
        if (FechaInicio.Date > FechaFin.Date)
        {
            Error = "La fecha inicial no puede ser mayor que la fecha final.";
            return Page();
        }
        try
        {
            Resultado = await _apiService.GetJsonAsync<ConsultaContribuyenteResultado>(
                $"/api/consultas/contribuyente?nit={Uri.EscapeDataString(Nit)}&tipo={Tipo}" +
                $"&fechaInicio={FechaInicio:yyyy-MM-dd}&fechaFin={FechaFin:yyyy-MM-dd}");
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return Page();
    }
}
