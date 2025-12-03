using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using escuchify_api.Modelos.Dtos;

namespace escuchify_api.Servicios;

public class SpotifyService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SpotifyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        var clientId = _configuration["Spotify:ClientId"];
        var clientSecret = _configuration["Spotify:ClientSecret"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret)) return null;

        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        try 
        {
            var response = await _httpClient.PostAsync("https://accounts.spotify.com/api/token", content);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }
        catch { return null; }
    }

    // Buscar Artista (Solo imagen)
    public async Task<string?> BuscarImagenArtista(string nombreArtista)
    {
        var token = await GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(nombreArtista)}&type=artist&limit=1");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        
        try 
        {
            var items = doc.RootElement.GetProperty("artists").GetProperty("items");
            if (items.GetArrayLength() > 0)
            {
                var imagenes = items[0].GetProperty("images");
                if (imagenes.GetArrayLength() > 0) return imagenes[0].GetProperty("url").GetString();
            }
        }
        catch { }
        return null;
    }

    // Buscar Disco (Info completa)
    public async Task<SpotifyDiscoDto?> BuscarInfoDisco(string tituloDisco, string nombreArtista)
    {
        var token = await GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var query = $"album:{tituloDisco} artist:{nombreArtista}";
        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=album&limit=1");
        
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        
        try 
        {
            var items = doc.RootElement.GetProperty("albums").GetProperty("items");
            if (items.GetArrayLength() > 0)
            {
                var item = items[0];
                var resultado = new SpotifyDiscoDto();

                // Imagen
                var imagenes = item.GetProperty("images");
                if (imagenes.GetArrayLength() > 0) resultado.ImagenUrl = imagenes[0].GetProperty("url").GetString();

                // Año
                if (item.TryGetProperty("release_date", out var dateElem))
                {
                    var fecha = dateElem.GetString();
                    if (!string.IsNullOrEmpty(fecha) && fecha.Length >= 4 && int.TryParse(fecha.Substring(0, 4), out int anio))
                        resultado.AnioLanzamiento = anio;
                }

                // Tipo
                if (item.TryGetProperty("album_type", out var typeElem))
                {
                    var tipo = typeElem.GetString() ?? "";
                    if (tipo.Length > 0) resultado.TipoDisco = char.ToUpper(tipo[0]) + tipo.Substring(1);
                }

                return resultado;
            }
        }
        catch { }
        return null;
    }
}