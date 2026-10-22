using IPC2_Proy03.Api.Modelos;
using System.Text.Json;

namespace IPC2_Proy03.Api.Servicios
{
    public class RtuService
    {
        //este servicio se encarga de admiistrar los contribuyentes
        //registrados en el RTU

        private readonly NitService _nitService;

            //ruta donde gardaremos los contribuyentes

            private readonly string _rutaArchivo;

            public RtuService(NitService nitService, IWebHostEnvironment environment)
        {

            //guardaremos el servicio de Nit para poder validar cada contribuytente recibido
            _nitService = nitService;


            //creamos la ruta
            // proyecto/Datos/Contribuyentes.json

            _rutaArchivo = Path.Combine(environment.ContentRootPath, "Datos", "Contribuyentes.json");

            //si la carpeta no existe, la creamos
            string? carpeta = Path.GetDirectoryName(_rutaArchivo);

            if (!string.IsNullOrEmpty(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            //si el archivo todavia no existe, lo inciamos con la lista vacia

            if (!File.Exists(_rutaArchivo))
            {
                File.WriteAllText(_rutaArchivo, "[]");

            }

        }

        //este metdo carga los contribuyentes que ya tenemos almacenados
        private List<Contribuyente> ObtenerContribuyentes()
        {
            string json = File.ReadAllText(_rutaArchivo);
            List<Contribuyente>? contribuyentes = JsonSerializer.Deserialize<List<Contribuyente>>(json);
            return contribuyentes ?? new List<Contribuyente>();
        }


        //guardar nuevamente todos los contribuyentes
        private void GuardarContribuyentes(List<Contribuyente> contribuyentes)
        {
            JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(contribuyentes, opciones);
            File.WriteAllText(_rutaArchivo, json);
        }

        //procesa el arreglo de contribuyentes
        //recibido desde la APP
        public ResultadosRtu ProcesarRtu(List<Contribuyente> contribuyentesRecibidos)
        {
        lock (SistemaService.BloqueoDatos)
        {
                //cargamos los que existen
                List<Contribuyente> contribuyentesAlmacenados = ObtenerContribuyentes();

                int nuevos = 0;
                int actualizados = 0;
                int invalidos = 0;



                //revisamos uno por uno los contribuyentes recibidos
                foreach (Contribuyente recibido in contribuyentesRecibidos)
                {
                    //primero validamos el NPT del contribuyente recibido
                    if (!_nitService.EsNitValido(recibido.Nit)) {

                        invalidos++;
                        continue;


                    }
                    recibido.Nit = NitService.Normalizar(recibido.Nit);
                    //buscamos si ya existe el contribuyente en la lista de almacenados

                    Contribuyente? existente = null;

                    foreach (Contribuyente guardado in contribuyentesAlmacenados)
                    {
                        if (NitService.Normalizar(guardado.Nit) == recibido.Nit)
                        {
                            existente = guardado;
                            break;
                        }
                    }

                    //si existe, actualizamos su nombre
                    if (existente == null)
                    {
                        contribuyentesAlmacenados.Add(recibido);

                        nuevos++;
                    }
                    else
                    {
                        //si ya existe, actualizamos su nombre
                        existente.Nombre = recibido.Nombre;
                        actualizados++;
                    }
                }

                //guardamos el estado final de RTU
                GuardarContribuyentes(contribuyentesAlmacenados);

                //regresmos exactamente los contadores
                //esperados por el cliente de la APP
                return new ResultadosRtu
                {
                    CONTRIBUYENTES_NUEVOS = nuevos,
                    CONTRIBUYENTES_ACTUALIZADOS = actualizados,
                    NITS_INVALIDOS = invalidos
                };
        }
    }
    }
}
