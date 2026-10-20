namespace IPC2_Proy03.Api.Modelos
{
    //ESTA ES LA RESPUSETA FINAL QUE RESIVIRA EL CLIENTE DESPUES DE PROCESAR LA SOLICITUD DE AUTORIZACION DE FACTURAS
    public class ResultadoAutorizaciones
    {
     public ResultadoAutorizacionesDetalle resultadoAutorizaciones { get; set; } = new ResultadoAutorizacionesDetalle();
    }
}