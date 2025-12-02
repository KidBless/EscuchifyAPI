using escuchify_api.Modelos;
using escuchify_api.Repositorios;

namespace escuchify_api.Servicios;

public class DiscoService
{
    private readonly IDiscoRepository _discoRepository;
    private readonly IArtistaRepository _artistaRepository; // Necesario para validar que el artista exista

    public DiscoService(IDiscoRepository discoRepository, IArtistaRepository artistaRepository)
    {
        _discoRepository = discoRepository;
        _artistaRepository = artistaRepository;
    }

    public List<Disco> ObtenerTodos()
    {
        return _discoRepository.ObtenerTodos();
    }

    public Disco? ObtenerPorId(int id)
    {
        return _discoRepository.ObtenerPorId(id);
    }

    public void CrearDisco(Disco disco)
    {
        if (string.IsNullOrWhiteSpace(disco.Titulo))
            throw new ArgumentException("El título del disco es obligatorio.");

        if (disco.AnioLanzamiento < 1800 || disco.AnioLanzamiento > DateTime.Now.Year + 1)
             throw new ArgumentException("El año de lanzamiento no es válido.");
        
        // Validación: El Artista debe existir
        var artista = _artistaRepository.ObtenerPorId(disco.ArtistaId);
        if (artista == null)
            throw new ArgumentException($"El artista con ID {disco.ArtistaId} no existe.");

        _discoRepository.Crear(disco);
    }

    public void ActualizarDisco(int id, Disco discoActualizado)
    {
        var discoExistente = _discoRepository.ObtenerPorId(id);
        if (discoExistente == null)
            throw new KeyNotFoundException($"Disco con ID {id} no encontrado.");

        discoExistente.Titulo = discoActualizado.Titulo;
        discoExistente.AnioLanzamiento = discoActualizado.AnioLanzamiento;
        discoExistente.TipoDisco = discoActualizado.TipoDisco;
        // Si permites cambiar de artista, valida de nuevo el ArtistaId aquí.

        _discoRepository.Actualizar(discoExistente);
    }

    public void EliminarDisco(int id)
    {
        var disco = _discoRepository.ObtenerPorId(id);
        if (disco != null)
        {
            _discoRepository.Eliminar(disco);
        }
    }
} 