namespace IPC2_Proy03.Api.Modelos;

//representa exacatamente una solicitud de autorizacion 
//enviada por un proveedor al backend 

public class SolicitudAutorizacion
{
    //"Guatemala, 15/01/2026 15:25 hrs" fecha y hora en que se envia la solicitud
    public string TIEMPO { get; set; } = string.Empty;

    //codigo interno enviado por el emisor 
    // debe ser unico en todo el sistema 
    public string REFERENCIA { get; set; } = string.Empty;

    public string NIT_EMISOR { get; set; } = string.Empty;

    public string NIT_RECEPTOR { get; set; } = string.Empty;


    //USAMOS DECIMAL PORQUE ESTAMOS TRABAJANDO CON VALORES MONETARIOS 

    public decimal VALOR { get; set; } 

    public decimal IVA { get; set; }

    public decimal TOTAL { get; set; }



}
