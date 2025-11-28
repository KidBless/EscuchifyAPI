using escuchify_api.Modelos;
using escuchify_api.Repositorios;

namespace escuchify_api.Servicios;

public class CancionService
{
    private readonly ICancionRepository _cancionRepository;
    private readonly IDiscoRepository _discoRepository; // Para validar que el disco exista

    public CancionService(ICancionRepository cancionRepository, IDiscoRepository discoRepository)
    {
        _cancionRepository = cancionRepository;
        _discoRepository = discoRepository;
    }

    public List<Cancion> ObtenerTodas() => _cancionRepository.ObtenerTodas();

    public Cancion? ObtenerPorId(int id) => _cancionRepository.ObtenerPorId(id);

    public void CrearCancion(Cancion cancion)
    {
        if (string.IsNullOrWhiteSpace(cancion.Titulo))
            throw new ArgumentException("El título de la canción es obligatorio.");

        // Validar que el disco exista
        var disco = _discoRepository.ObtenerPorId(cancion.DiscoId);
        if (disco == null)
            throw new ArgumentException($"El disco con ID {cancion.DiscoId} no existe.");

        _cancionRepository.Crear(cancion);
    }

    public void ActualizarCancion(int id, Cancion cancionActualizada)
    {
        var cancion = _cancionRepository.ObtenerPorId(id);
        if (cancion == null)
            throw new KeyNotFoundException($"Canción con ID {id} no encontrada.");

        cancion.Titulo = cancionActualizada.Titulo;
        cancion.Duracion = cancionActualizada.Duracion;
        cancion.Genero = cancionActualizada.Genero;
        
        _cancionRepository.Actualizar(cancion);
    }

    public void EliminarCancion(int id)
    {
        var cancion = _cancionRepository.ObtenerPorId(id);
        if (cancion != null)
        {
            _cancionRepository.Eliminar(cancion);
        }
    }
}