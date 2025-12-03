using escuchify_api.Modelos;

namespace escuchify_api.Servicios;

public interface IArtistaService
{
    List<Artista> ObtenerTodos();
    Artista? ObtenerPorId(int id);
    
    // Método asíncrono para permitir la llamada a API externa
    Task RegistrarArtista(Artista artista);
    
    void ActualizarArtista(int id, Artista artista);
    void EliminarArtista(int id);
    Task<string?> ObtenerImagenSpotify(string nombreArtista);
}