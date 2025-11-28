using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public interface ICancionRepository
{
    List<Cancion> ObtenerTodas();
    Cancion? ObtenerPorId(int id);
    void Crear(Cancion cancion);
    void Actualizar(Cancion cancion);
    void Eliminar(Cancion cancion);
}