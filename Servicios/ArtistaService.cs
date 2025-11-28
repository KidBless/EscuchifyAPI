using escuchify_api.Modelos;
using escuchify_api.Repositorios;

namespace escuchify_api.Servicios;

public class ArtistaService
{
    private readonly IArtistaRepository _repository;

    public ArtistaService(IArtistaRepository repository)
    {
        _repository = repository;
    }

    public List<Artista> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Artista? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public void RegistrarArtista(Artista artista)
    {
        // Regla de Negocio: Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(artista.NombreArtistico))
        {
            throw new ArgumentException("El nombre artístico es obligatorio.");
        }

        // Regla de Negocio: No permitir artistas duplicados (por nombre)
        var existentes = _repository.ObtenerTodos();
        if (existentes.Any(a => a.NombreArtistico.ToLower() == artista.NombreArtistico.ToLower()))
        {
            throw new InvalidOperationException($"El artista '{artista.NombreArtistico}' ya existe en la base de datos.");
        }

        _repository.Crear(artista);
    }

    public void ActualizarArtista(int id, Artista artistaActualizado)
    {
        var artistaExistente = _repository.ObtenerPorId(id);
        if (artistaExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró el artista con ID {id}");
        }

        // Actualizamos los campos permitidos
        artistaExistente.NombreArtistico = artistaActualizado.NombreArtistico;
        artistaExistente.NombreCompleto = artistaActualizado.NombreCompleto;
        artistaExistente.Nacionalidad = artistaActualizado.Nacionalidad;
        // ... otros campos ...

        _repository.Actualizar(artistaExistente);
    }

    public void EliminarArtista(int id)
    {
        var artista = _repository.ObtenerPorId(id);
        if (artista == null) return; // O lanzar excepción según prefieras

        _repository.Eliminar(artista);
    }
}