namespace escuchify_api.Modelos;

public class Disco
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int AnioLanzamiento { get; set; }
    public string TipoDisco { get; set; }

    // Relación con Artista
    public int ArtistaId { get; set; }
    public Artista? Artista { get; set; }

    public List<Cancion> Canciones { get; set; } = new List<Cancion>();

    // Campo nuevo para la portada
    public string? ImagenUrl { get; set; }

    public Disco() { }

    public Disco(int id, string titulo, int anioLanzamiento, string tipoDisco)
    {
        Id = id;
        Titulo = titulo;
        AnioLanzamiento = anioLanzamiento;
        TipoDisco = tipoDisco;
    }
}