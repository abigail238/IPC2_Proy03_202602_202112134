namespace IPC2_Proy03.Api.Modelos;

//esta clase representa la respuesta que devolvera la API
//despues de procesar una actualizacion de RTU

public class ResultadosRtu
{
    //cantidad de contribuyentes nuevos agregados 
    public int  CONTRIBUYENTES_NUEVOS { get; set; }

    //cantidad de contribuyentes que ya existian y fueron actualizados
    public int CONTRIBUYENTES_ACTUALIZADOS { get; set; }

    //contidad de nit que no pasaron la validacion 
    public int NITS_INVALIDOS { get; set; }
}