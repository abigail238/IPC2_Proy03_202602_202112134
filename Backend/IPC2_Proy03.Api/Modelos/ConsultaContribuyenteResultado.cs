namespace IPC2_Proy03.Api.Modelos
{

    // Representa el resultado completo de consultar
    // las facturas emitidas o recibidas por un NIT.
    public class ConsultaContribuyenteResultado
    {
        // NIT que se consultó.
        public string Nit { get; set; } = string.Empty;

        // Indica si la consulta corresponde a facturas
        // emitidas o recibidas.
        public string Tipo { get; set; } = string.Empty;

        // Información agrupada por cada fecha.
        public List<ResumenFechaContribuyente> Detalle { get; set; } = [];

        // Total facturado durante todo el rango consultado.
        public decimal TotalFacturado { get; set; }
    }
}
