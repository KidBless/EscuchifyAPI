using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public interface IArtistaRepository
{
    List<Artista> ObtenerTodos();
    Artista? ObtenerPorId(int id);
    void Crear(Artista artista);
    void Actualizar(Artista artista);
    void Eliminar(Artista artista);
}