using System.Text;
using System.Text.Json;
namespace IPC2_Proy03.Frontend.Servicios;
public class ApiService
{
    private readonly HttpClient _httpClient;
    public ApiService(HttpClient httpCliente, IConfiguration configuration)
    {
        string? baseUrl = configuration["ApiSettings:BaseUrl"];
        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var direccion) || (direccion.Scheme != "http" && direccion.Scheme != "https"))
            throw new InvalidOperationException("Debe configurar una URL HTTP válida para el backend.");
        _httpClient = httpCliente;
        _httpClient.BaseAddress = direccion;
    }
    public async Task<string> PostJsonAsync(string endpoint, string json)
    {
        using var contenido = new StringContent(json, Encoding.UTF8, "application/json");
        using var respuesta = await _httpClient.PostAsync(endpoint, contenido);
        return await LeerRespuestaAsync(respuesta);
    }
    public async Task<string> GetAsync(string endpoint)
    {
        using var respuesta = await _httpClient.GetAsync(endpoint);
        return await LeerRespuestaAsync(respuesta);
    }
    public async Task<T?> GetJsonAsync<T>(string endpoint)
    {
        string json = await GetAsync(endpoint);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
    public async Task<string> DeleteAsync(string endpoint)
    {
        using var respuesta = await _httpClient.DeleteAsync(endpoint);
        return await LeerRespuestaAsync(respuesta);
    }
    public Task<string> GetTask(string endpoint) => GetAsync(endpoint);
    public Task<string> TaskAsync(string endpoint) => DeleteAsync(endpoint);
    private static async Task<string> LeerRespuestaAsync(HttpResponseMessage respuesta)
    {
        string resultado = await respuesta.Content.ReadAsStringAsync();
        if (!respuesta.IsSuccessStatusCode)
            throw new HttpRequestException(resultado, null, respuesta.StatusCode);
        return resultado;
    }

    
    // DESCARGAR ARCHIVO
   

    public async Task<byte[]> DescargarArchivoAsync(string endpoint)
    {
        using HttpResponseMessage respuesta = await _httpClient.GetAsync(endpoint);

        if (!respuesta.IsSuccessStatusCode)
        {
            string error =
                await respuesta.Content.ReadAsStringAsync();

            throw new Exception(error);
        }

        return await respuesta.Content.ReadAsByteArrayAsync();
    }

}
