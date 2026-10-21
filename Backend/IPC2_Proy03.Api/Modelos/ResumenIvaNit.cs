namespace IPC2_Proy03.Api.Modelos
{
    public class ResumenIvaNit
    {

        // Representa cuánto IVA emitió y cuánto IVA recibió
        // un contribuyente en una fecha determinad
            public string Nit { get; set; } = string.Empty;

            // IVA correspondiente a facturas donde este NIT
            // aparece como emisor.
            public decimal IvaEmitido { get; set; }

            // IVA correspondiente a facturas donde este NIT
            // aparece como receptor.
            public decimal IvaRecibido { get; set; }
        }
    }

