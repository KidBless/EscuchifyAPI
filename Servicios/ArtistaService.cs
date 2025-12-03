using escuchify_api.Modelos;
using escuchify_api.Repositorios;

namespace escuchify_api.Servicios;

public class ArtistaService : IArtistaService
{
    private readonly IArtistaRepository _repository;
    private readonly SpotifyService _spotify; // Inyectamos el servicio de Spotify

public async Task<string?> ObtenerImagenSpotify(string nombreArtista)
{
    return await _spotify.BuscarImagenArtista(nombreArtista);
}

    public ArtistaService(IArtistaRepository repository, SpotifyService spotify)
    {
        _repository = repository;
        _spotify = spotify;
    }

    public List<Artista> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Artista? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    // Este método ahora es ASYNC para poder llamar a Spotify
    public async Task RegistrarArtista(Artista artista)
    {
        // 1. Validaciones de Negocio
        if (string.IsNullOrWhiteSpace(artista.NombreArtistico))
        {
            throw new ArgumentException("El nombre artístico es obligatorio.");
        }

        var existentes = _repository.ObtenerTodos();
        if (existentes.Any(a => a.NombreArtistico.ToLower() == artista.NombreArtistico.ToLower()))
        {
            throw new InvalidOperationException($"El artista '{artista.NombreArtistico}' ya existe.");
        }

        // 2. Intentar obtener imagen de Spotify (Auto-completado)
        try
        {
            var imagenUrl = await _spotify.BuscarImagenArtista(artista.NombreArtistico);
            
            // Si Spotify nos devuelve una URL, la asignamos al artista
            if (!string.IsNullOrEmpty(imagenUrl))
            {
                artista.ImagenUrl = imagenUrl;
            }
        }
        catch (Exception ex)
        {
            // Si falla la conexión con Spotify, NO detenemos el registro.
            // Simplemente guardamos el artista sin foto y avisamos en consola.
            Console.WriteLine($"Advertencia: No se pudo obtener imagen de Spotify. Error: {ex.Message}");
        }

        // 3. Guardar en Base de Datos
        _repository.Crear(artista);
    }

    public void ActualizarArtista(int id, Artista artistaActualizado)
    {
        var artistaExistente = _repository.ObtenerPorId(id);
        if (artistaExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró el artista con ID {id}");
        }

        // Actualizamos los campos
        artistaExistente.NombreArtistico = artistaActualizado.NombreArtistico;
        artistaExistente.NombreCompleto = artistaActualizado.NombreCompleto;
        artistaExistente.Nacionalidad = artistaActualizado.Nacionalidad;
        artistaExistente.AnioInicio = artistaActualizado.AnioInicio;
        artistaExistente.Discografica = artistaActualizado.Discografica;
        
        // Si el usuario provee una imagen manualmente al editar, la actualizamos
        if (!string.IsNullOrEmpty(artistaActualizado.ImagenUrl))
        {
            artistaExistente.ImagenUrl = artistaActualizado.ImagenUrl;
        }

        _repository.Actualizar(artistaExistente);
    }

    public void EliminarArtista(int id)
    {
        var artista = _repository.ObtenerPorId(id);
        if (artista != null)
        {
            _repository.Eliminar(artista);
        }
    }
}