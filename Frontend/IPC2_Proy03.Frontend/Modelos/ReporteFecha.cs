namespace IPC2_Proy03.Frontend.Modelos;

// Representa las estadísticas que devuelve
// el backend para una fecha determinada.
public class ReporteFecha
{
    public DateTime Fecha { get; set; }

    // Cantidad de facturas enviadas al servicio.
    public int CantidadTotalFacturasRecibidas { get; set; }

    // Errores encontrados.
    public int NitEmisorInvalido { get; set; }

    public int NitReceptorInvalido { get; set; }

    public int NitEmisorInexistente { get; set; }

    public int NitReceptorInexistente { get; set; }

    public int IvaMalCalculado { get; set; }

    public int TotalMalCalculado { get; set; }

    public int ReferenciaDuplicada { get; set; }

    // Facturas aprobadas correctamente.
    public int CantidadFacturasSinErrores { get; set; }

    // Cantidad de NIT distintos.
    public int CantidadEmisores { get; set; }

    public int CantidadReceptores { get; set; }
}