namespace IPC2_Proy03.Api.Modelos
{

    // Representa el valor autorizado durante una fecha.
    // Se utiliza para el resumen por rango.
    public class ResumenRangoFecha
    {

        public DateTime Fecha { get; set; }

        // Puede representar TOTAL o VALOR sin IVA,
        // dependiendo de la opción solicitada.
        public decimal ValorAutorizado { get; set; }
    }
}
