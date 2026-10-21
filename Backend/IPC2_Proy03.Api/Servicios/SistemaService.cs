namespace IPC2_Proy03.Api.Servicios;

// Servicio encargado de devolver el sistema
// a su estado inicial.
public class SistemaService
{
    private readonly string _rutaContribuyentes;
    private readonly string _rutaAutorizaciones;

    public SistemaService(
        IWebHostEnvironment environment)
    {
        // Ruta del archivo que contiene el RTU.
        _rutaContribuyentes = Path.Combine(
            environment.ContentRootPath,
            "Datos",
            "Contribuyentes.json"
        );

        // Ruta del archivo que contiene
        // autorizaciones y solicitudes procesadas.
        _rutaAutorizaciones = Path.Combine(
            environment.ContentRootPath,
            "Datos",
            "Autorizaciones.json"
        );

        // Nos aseguramos de que exista la carpeta Datos.
        string rutaDatos =
            Path.Combine(
                environment.ContentRootPath,
                "Datos"
            );

        Directory.CreateDirectory(rutaDatos);
    }

    // Borra lógicamente todos los datos
    // dejando nuevamente listas JSON vacías.
    public void Inicializar()
    {
        File.WriteAllText(
            _rutaContribuyentes,
            "[]"
        );

        File.WriteAllText(
            _rutaAutorizaciones,
            "[]"
        );
    }
}