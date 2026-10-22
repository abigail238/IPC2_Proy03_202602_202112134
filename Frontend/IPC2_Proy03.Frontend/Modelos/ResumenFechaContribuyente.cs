namespace IPC2_Proy03.Frontend.Modelos;

// Representa el resumen de las facturas de un
// contribuyente para una fecha específica.
public class ResumenFechaContribuyente
{
    // Día al que pertenecen las facturas.
    public DateTime Fecha { get; set; }

    // Cantidad de facturas autorizadas durante ese día.
    public int CantidadFacturas { get; set; }

    // Suma del total de las facturas de ese día.
    public decimal MontoTotal { get; set; }
}