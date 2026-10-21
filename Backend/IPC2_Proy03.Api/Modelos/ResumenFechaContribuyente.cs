namespace IPC2_Proy03.Api.Modelos
{

    // Representa el resumen de facturas de un contribuyente
    // para un día específico.
    public class ResumenFechaContribuyente
    {
        // Día al que pertenece el resumen.
        public DateTime Fecha { get; set; }

        // Cantidad de facturas autorizadas durante ese día.
        public int CantidadFacturas { get; set; }

        // Suma del TOTAL de las facturas autorizadas.
        public decimal MontoTotal { get; set; }

    }
}
