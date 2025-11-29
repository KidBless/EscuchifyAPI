namespace escuchify_api.Modelos;

public class Disco
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int AnioLanzamiento { get; set; }
    public string TipoDisco { get; set; }

    // --- AGREGAR ESTO (Relación con Artista) ---
    public int ArtistaId { get; set; } // La clave foránea
    public Artista? Artista { get; set; } // La navegación
    // ------------------------------------------

    public List<Cancion> Canciones { get; set; } = new List<Cancion>();

    // Constructor vacío requerido por EF Core
    // public Disco() { }

    public Disco(int id, string titulo, int anioLanzamiento, string tipoDisco)
    {
        Id = id;
        Titulo = titulo;
        AnioLanzamiento = anioLanzamiento;
        TipoDisco = tipoDisco;
    }
}