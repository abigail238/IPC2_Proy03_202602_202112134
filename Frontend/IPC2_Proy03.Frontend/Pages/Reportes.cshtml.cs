using IPC2_Proy03.Frontend.Modelos;
using IPC2_Proy03.Frontend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class ReportesModel : PageModel
{
    private readonly ApiService _apiService;

    public ReportesModel(
        ApiService apiService)
    {
        _apiService = apiService;
    }


    [BindProperty]
    public DateTime FechaInicio { get; set; }
        = DateTime.Today.AddMonths(-1);


    [BindProperty]
    public DateTime FechaFin { get; set; }
        = DateTime.Today;


    public List<ReporteFecha> Resultado { get; set; }
        = [];


    public string? Error { get; set; }


    public void OnGet()
    {
    }


    // MOSTRAR REPORTE EN PANTALLA

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (FechaInicio.Date > FechaFin.Date)
        {
            Error =
                "La fecha inicial no puede ser mayor que la fecha final.";

            return Page();
        }


        try
        {
            string endpoint =
                $"/api/reportes" +
                $"?fechaInicio={FechaInicio:yyyy-MM-dd}" +
                $"&fechaFin={FechaFin:yyyy-MM-dd}";


            Resultado = await _apiService.GetJsonAsync<List<ReporteFecha>>(endpoint) ?? [];
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }


        return Page();
    }


    // GENERAR PDF
   
    public async Task<IActionResult> OnPostPdfAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (FechaInicio.Date > FechaFin.Date)
        {
            Error =
                "La fecha inicial no puede ser mayor que la fecha final.";

            return Page();
        }


        try
        {
            string endpoint =
                $"/api/reportes/pdf" +
                $"?fechaInicio={FechaInicio:yyyy-MM-dd}" +
                $"&fechaFin={FechaFin:yyyy-MM-dd}";


            byte[] pdf =
                await _apiService
                    .DescargarArchivoAsync(
                        endpoint
                    );


            string nombreArchivo =
                $"Reporte_{FechaInicio:yyyyMMdd}_{FechaFin:yyyyMMdd}.pdf";


            return File(
                pdf,
                "application/pdf",
                nombreArchivo
            );
        }
        catch (Exception ex)
        {
            Error = ex.Message;

            return Page();
        }
    }
}