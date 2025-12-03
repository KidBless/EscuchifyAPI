using System.Text.Json.Serialization; // Opcional: Para evitar ciclos si usas JSON directo

namespace escuchify_api.Modelos;

public class Artista
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string NombreArtistico { get; set; }
    public int AnioInicio { get; set; }
    public string Nacionalidad { get; set; }
    public string Discografica { get; set; }
    public string? ImagenUrl { get; set; }

    // --- RELACIÓN IMPORTANTE ---
    // Esta lista permite que un Artista tenga muchos Discos.
    // Inicializarla con 'new List<Disco>()' evita errores de "NullReference"
    public List<Disco> Discos { get; set; } = new List<Disco>();

    // Constructor vacío (Requerido por EF Core)
    // public Artista() { }

    // Constructor con parámetros (Para tu uso manual)
    public Artista(int id, string nombreCompleto, string nombreArtistico, int anioInicio, string nacionalidad, string discografica)
    {
        Id = id;
        NombreCompleto = nombreCompleto;
        NombreArtistico = nombreArtistico;
        AnioInicio = anioInicio;
        Nacionalidad = nacionalidad;
        Discografica = discografica;
    }

    public override string ToString()
    {
        return $"{NombreArtistico} ({AnioInicio})";
    }
}