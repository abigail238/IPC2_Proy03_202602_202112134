using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proy03.Frontend.Pages;

public class AyudaModel : PageModel
{
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
}