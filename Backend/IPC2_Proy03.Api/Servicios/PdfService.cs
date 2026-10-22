using IPC2_Proy03.Api.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IPC2_Proy03.Api.Servicios;

// Servicio encargado de transformar
// los reportes en documentos PDF.
public class PdfService
{
    public byte[] GenerarReporte(
        List<ReporteFecha> reportes,
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        // Creamos un documento PDF.
        var documento =
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Tamaño estándar A4.
                    page.Size(PageSizes.A4);

                    // Márgenes.
                    page.Margin(30);

                    // Tamaño de letra predeterminado.
                    page.DefaultTextStyle(
                        x => x.FontSize(10)
                    );


                    // -----------------------------------------
                    // ENCABEZADO
                    // -----------------------------------------

                    page.Header()
                        .Column(columna =>
                        {
                            columna.Item()
                                .Text(
                                    "Reporte de Documentos Tributarios Electrónicos"
                                )
                                .Bold()
                                .FontSize(18);

                            columna.Item()
                                .Text(
                                    $"Rango: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}"
                                );
                        });


                    // -----------------------------------------
                    // CONTENIDO
                    // -----------------------------------------

                    page.Content()
                        .PaddingVertical(15)
                        .Column(columna =>
                        {
                            columna.Spacing(15);


                            // Si no existen registros,
                            // indicamos que no hay información.
                            if (reportes.Count == 0)
                            {
                                columna.Item()
                                    .Text(
                                        "No existen datos para el rango seleccionado."
                                    );

                                return;
                            }


                            foreach (ReporteFecha reporte in reportes)
                            {
                                columna.Item()
                                    .Text(
                                        $"Fecha: {reporte.Fecha:dd/MM/yyyy}"
                                    )
                                    .Bold()
                                    .FontSize(13);


                                columna.Item()
                                    .Text(
                                        $"Facturas recibidas: {reporte.CantidadTotalFacturasRecibidas}"
                                    );


                                columna.Item()
                                    .Text(
                                        $"Facturas sin errores: {reporte.CantidadFacturasSinErrores}"
                                    );


                                columna.Item()
                                    .Text(
                                        $"Emisores distintos: {reporte.CantidadEmisores}"
                                    );


                                columna.Item()
                                    .Text(
                                        $"Receptores distintos: {reporte.CantidadReceptores}"
                                    );


                                // Tabla de errores.
                                columna.Item()
                                    .Table(tabla =>
                                    {
                                        tabla.ColumnsDefinition(
                                            columnas =>
                                            {
                                                columnas.RelativeColumn(3);
                                                columnas.RelativeColumn(1);
                                            }
                                        );


                                        tabla.Header(header =>
                                        {
                                            header.Cell()
                                                .Padding(5)
                                                .Text("Tipo de error")
                                                .Bold();

                                            header.Cell()
                                                .Padding(5)
                                                .Text("Cantidad")
                                                .Bold();
                                        });


                                        AgregarFila(
                                            tabla,
                                            "NIT emisor inválido",
                                            reporte.NitEmisorInvalido
                                        );

                                        AgregarFila(
                                            tabla,
                                            "NIT receptor inválido",
                                            reporte.NitReceptorInvalido
                                        );

                                        AgregarFila(
                                            tabla,
                                            "NIT emisor inexistente",
                                            reporte.NitEmisorInexistente
                                        );

                                        AgregarFila(
                                            tabla,
                                            "NIT receptor inexistente",
                                            reporte.NitReceptorInexistente
                                        );

                                        AgregarFila(
                                            tabla,
                                            "IVA mal calculado",
                                            reporte.IvaMalCalculado
                                        );

                                        AgregarFila(
                                            tabla,
                                            "TOTAL mal calculado",
                                            reporte.TotalMalCalculado
                                        );

                                        AgregarFila(
                                            tabla,
                                            "REFERENCIA duplicada",
                                            reporte.ReferenciaDuplicada
                                        );
                                    });
                            }
                        });


                    // -----------------------------------------
                    // PIE DE PÁGINA
                    // -----------------------------------------

                    page.Footer()
                        .AlignCenter()
                        .Text(texto =>
                        {
                            texto.Span("Página ");

                            texto.CurrentPageNumber();

                            texto.Span(" de ");

                            texto.TotalPages();
                        });
                });
            });


        // GeneratePdf() sin ruta devuelve
        // directamente el PDF como byte[].
        return documento.GeneratePdf();
    }


    // Método auxiliar para agregar una fila
    // a la tabla de errores.
    private static void AgregarFila(
        TableDescriptor tabla,
        string descripcion,
        int cantidad)
    {
        tabla.Cell()
            .Padding(5)
            .Text(descripcion);

        tabla.Cell()
            .Padding(5)
            .Text(cantidad.ToString());
    }
}