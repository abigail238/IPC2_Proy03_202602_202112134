namespace IPC2_Proy03.Api.Interfaces
{

    //cualquier clase que quiera ser un serivicio de nit debera saber validar un nit 

    //esta interfaz nos servira para definir los metodos que se implementaran en la clase NitService
    public interface INitService
    {
        bool EsNitValido(string nit);


    }
}
