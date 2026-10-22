using System.Globalization;
using System.Text.RegularExpressions;

namespace IPC2_Proy03.Api.Utils;


    //clase para extraer fechas de texto

    public static class FechaParser
{
    public static DateTime? ExtraerFecha(string? texto)
    {

        // si no viene texto, no podemos obetner la fecha
        if (string.IsNullOrEmpty(texto))
        {
            return null;
        }

        // buscamos especificamente el patron de fecha en el texto

        Regex expresion = new Regex(@"(?<!\d)\d{2}/\d{2}/\d{4}\s+\d{2}:\d{2}(?!\d)");

        Match coincidencia = expresion.Match(texto);

        if(!coincidencia.Success )
        {
            return null;

        }

        //intentamos convertir el texto encontrado
        //a un DateTime real

        bool convertido = DateTime.TryParseExact(coincidencia.Value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime fecha);
        if (!convertido)
        {
            return null;
        }

        return fecha;
    }
}

