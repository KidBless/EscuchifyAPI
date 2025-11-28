using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public interface IDiscoRepository
{
    List<Disco> ObtenerTodos();
    Disco? ObtenerPorId(int id);
    void Crear(Disco disco);
    void Actualizar(Disco disco);
    void Eliminar(Disco disco);
}