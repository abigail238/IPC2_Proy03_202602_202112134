using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Mvc;

namespace IPC2_Proy03.Frontend.Pages;

public class AyudaModel : PageModel
{
    private readonly IWebHostEnvironment _environment;

    public AyudaModel(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public static IReadOnlyDictionary<string, string> Documentos { get; } =
        new Dictionary<string, string>
        {
            ["Ensayo-IPC2-Py03.docx.pdf"] = "Ensayo del proyecto",
            ["Digrama de Clases.png"] = "Diagrama de clases",
            ["Diagrama de despliegue.png"] = "Diagrama de despliegue",
            ["Digrama de actividad.png"] = "Diagrama de actividad",
            ["Diagrama de actividad DTE.png"] = "Diagrama de actividad DTE",
            ["Diagrama de consulta de contribuyente.png"] = "Consulta de contribuyente",
            ["Digrama de generacion de reportes.png"] = "Generación de reportes"
        };

    // Datos mostrados en la sección de información
    // del estudiante.
    public string NombreEstudiante { get; set; }
        = "Lorena Abigail Arrecis Martinez";

    public string Carnet { get; set; }
        = "202112134";

    public string Curso { get; set; }
        = "Introducción a la Programación y Computación 2";

    public string Proyecto { get; set; }
        = "Proyecto 3";

    public void OnGet()
    {
    }

    public IActionResult OnGetDocumento(string? archivo)
    {
        // Solo se exponen los documentos del proyecto incluidos en esta lista.
        if (archivo is null || !Documentos.ContainsKey(archivo))
            return NotFound();

        string[] carpetas =
        [
            Path.Combine(_environment.ContentRootPath, "Documentacion"),
            Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "..", "..", "Documentacion")),
            Path.Combine(AppContext.BaseDirectory, "Documentacion")
        ];

        string? ruta = carpetas.Select(c => Path.Combine(c, archivo)).FirstOrDefault(System.IO.File.Exists);
        if (ruta is null)
            return NotFound();

        string tipoContenido = archivo.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
            ? "application/pdf" : "image/png";

        return new PhysicalFileResult(ruta, tipoContenido) { EnableRangeProcessing = true };
    }
}
