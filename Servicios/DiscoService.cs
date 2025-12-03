using escuchify_api.Modelos;
using escuchify_api.Modelos.Dtos;
using escuchify_api.Repositorios;
using Microsoft.EntityFrameworkCore; // Necesario para Include si usas EF directo o Repository

namespace escuchify_api.Servicios;

public class DiscoService
{
    private readonly IDiscoRepository _discoRepository;
    private readonly IArtistaRepository _artistaRepository;
    private readonly SpotifyService _spotify;

    public DiscoService(IDiscoRepository discoRepository, IArtistaRepository artistaRepository, SpotifyService spotify)
    {
        _discoRepository = discoRepository;
        _artistaRepository = artistaRepository;
        _spotify = spotify;
    }

    public List<Disco> ObtenerTodos() => _discoRepository.ObtenerTodos();
    
    public Disco? ObtenerPorId(int id) => _discoRepository.ObtenerPorId(id);

    public async Task<SpotifyDiscoDto?> BuscarDatosEnSpotify(string titulo, int artistaId)
    {
        var artista = _artistaRepository.ObtenerPorId(artistaId);
        string nombreArtista = artista?.NombreArtistico ?? "";
        return await _spotify.BuscarInfoDisco(titulo, nombreArtista);
    }

    public async Task CrearDisco(Disco disco)
    {
        // Validaciones básicas
        if (string.IsNullOrWhiteSpace(disco.Titulo)) throw new ArgumentException("El título es obligatorio.");
        
        // Si no tiene imagen, intentamos buscarla automáticamente (opcional)
        if (string.IsNullOrEmpty(disco.ImagenUrl))
        {
            try 
            {
                var info = await BuscarDatosEnSpotify(disco.Titulo, disco.ArtistaId);
                if (info != null)
                {
                    disco.ImagenUrl = info.ImagenUrl;
                    if (disco.AnioLanzamiento == 0) disco.AnioLanzamiento = info.AnioLanzamiento;
                    if (string.IsNullOrEmpty(disco.TipoDisco)) disco.TipoDisco = info.TipoDisco;
                }
            }
            catch { /* Continuar sin error si falla Spotify */ }
        }

        _discoRepository.Crear(disco);
    }

    public void ActualizarDisco(int id, Disco disco)
    {
        // Lógica de actualización (usar repositorio)
        var existente = _discoRepository.ObtenerPorId(id);
        if (existente == null) throw new KeyNotFoundException("Disco no encontrado");
        
        existente.Titulo = disco.Titulo;
        existente.AnioLanzamiento = disco.AnioLanzamiento;
        existente.TipoDisco = disco.TipoDisco;
        existente.ArtistaId = disco.ArtistaId;
        if(!string.IsNullOrEmpty(disco.ImagenUrl)) existente.ImagenUrl = disco.ImagenUrl;

        _discoRepository.Actualizar(existente);
    }

    public void EliminarDisco(int id)
    {
        var disco = _discoRepository.ObtenerPorId(id);
        if (disco != null) _discoRepository.Eliminar(disco);
    }
}