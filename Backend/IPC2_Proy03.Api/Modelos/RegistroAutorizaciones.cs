namespace IPC2_Proy03.Api.Modelos;

//respresenta el resultado persiste de procesar 
//una solicitud de autorizacion

public class RegistroAutorizaciones
{
    public string Referencia { get; set; } = string.Empty;

    public string NitEmisor { get; set; } = string.Empty;

    public string NitReceptor { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    //fecha extraida de Tiempo 

    public DateTime? Fecha { get; set; }


    // solo tendra valor caundo la factura haya sido aprobada
    public long? CodigoAprobacion { get; set; }

    public bool Aprobada { get; set; }

    //aqui guardaresmo todos los errores encontrados 

    //"IVA_MAL_CALCULADO"

    public List<string> Errores { get; set; } = [];


}