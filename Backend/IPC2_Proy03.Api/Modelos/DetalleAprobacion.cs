namespace IPC2_Proy03.Api.Modelos;

// Representa una factura que sí fue aprobada.
public class DetalleAprobacion
{

    public string referencia { get; set; } = string.Empty;

    // el codigo tiene 16 digitos:
    //yyyyMMdd + 8 DIGITOS DE CORRELATIVO 

    public long codigoAprobacion { get; set; }
}
