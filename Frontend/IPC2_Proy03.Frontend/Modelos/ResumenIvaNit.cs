namespace IPC2_Proy03.Frontend.Modelos;

// Representa el resumen del IVA correspondiente
// a un NIT durante una fecha determinada.
public class ResumenIvaNit
{
    // Contribuyente al que pertenece el resumen.
    public string Nit { get; set; } = string.Empty;

    // IVA de las facturas donde este NIT
    // aparece como emisor.
    public decimal IvaEmitido { get; set; }

    // IVA de las facturas donde este NIT
    // aparece como receptor.
    public decimal IvaRecibido { get; set; }
}