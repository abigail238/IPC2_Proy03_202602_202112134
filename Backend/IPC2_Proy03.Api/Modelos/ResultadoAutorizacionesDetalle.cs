namespace IPC2_Proy03.Api.Modelos
{
    

    public class ResultadoAutorizacionesDetalle
    {
        public int cantidadAutorizacionesAprobadas { get; set; }

        public int cantidadAutorizacionesRechazadasPorNitEmisorInexistente { get; set; }

        public int cantidadAutorizacionesRechazadasPorNitReceptorInexistente { get; set; }

        public List<DetalleAprobacion> detalleAprobaciones { get; set; } = [];
    }
}