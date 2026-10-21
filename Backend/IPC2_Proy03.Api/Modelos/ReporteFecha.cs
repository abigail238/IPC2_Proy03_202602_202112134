namespace IPC2_Proy03.Api.Modelos;

// Representa las estadísticas correspondientes
// a una fecha específica.
public class ReporteFecha
{
    public DateTime Fecha { get; set; }

    public int CantidadTotalFacturasRecibidas { get; set; }

    public int NitEmisorInvalido { get; set; }

    public int NitReceptorInvalido { get; set; }

    public int NitEmisorInexistente { get; set; }

    public int NitReceptorInexistente { get; set; }

    public int IvaMalCalculado { get; set; }

    public int TotalMalCalculado { get; set; }

    public int ReferenciaDuplicada { get; set; }

    public int CantidadFacturasSinErrores { get; set; }

    public int CantidadEmisores { get; set; }

    public int CantidadReceptores { get; set; }
}