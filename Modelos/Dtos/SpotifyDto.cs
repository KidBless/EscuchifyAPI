namespace escuchify_api.Modelos.Dtos;

public class SpotifyDiscoDto
{
    public string? ImagenUrl { get; set; }
    public int AnioLanzamiento { get; set; }
    public string TipoDisco { get; set; } = string.Empty;
}