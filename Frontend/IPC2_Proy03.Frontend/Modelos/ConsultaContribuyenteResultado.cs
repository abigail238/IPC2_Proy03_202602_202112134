namespace IPC2_Proy03.Frontend.Modelos;

// Representa la respuesta completa que devuelve
// el backend al consultar un contribuyente.
public class ConsultaContribuyenteResultado
{
	// NIT que consultó el usuario.
	public string Nit { get; set; } = string.Empty;

	// Puede ser:
	//
	// emitidas
	// recibidas
	public string Tipo { get; set; } = string.Empty;

	// Contiene el resumen agrupado por fecha.
	public List<ResumenFechaContribuyente> Detalle { get; set; } = [];

	// Suma total de todo el rango consultado.
	public decimal TotalFacturado { get; set; }
}