using escuchify_api.Modelos;
using escuchify_api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace escuchify_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistasController : ControllerBase
{
    private readonly IArtistaService _service; // Usamos la interfaz para desacoplar

    public ArtistasController(IArtistaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos());
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerPorId(int id)
    {
        var artista = _service.ObtenerPorId(id);
        if (artista == null)
        {
            return NotFound("Artista no encontrado.");
        }
        return Ok(artista);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Artista artista)
    {
        try
        {
            // Llamamos al servicio con 'await' porque ahora busca en Spotify
            await _service.RegistrarArtista(artista);
            
            // Retornamos 201 Created
            return CreatedAtAction(nameof(ObtenerPorId), new { id = artista.Id }, artista);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400 Bad Request
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // 409 Conflict (si ya existe)
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Actualizar(int id, [FromBody] Artista artista)
    {
        try
        {
            _service.ActualizarArtista(id, artista);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        _service.EliminarArtista(id);
        return NoContent();
    }

    [HttpGet("buscar-imagen")]
    public async Task<IActionResult> BuscarImagen([FromQuery] string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest("Debes enviar un nombre.");

        var url = await _service.ObtenerImagenSpotify(nombre);
        
        if (url == null) 
            return NotFound("No se encontró imagen para este artista.");

        // Devolvemos un objeto simple con la URL
        return Ok(new { Url = url });
    }
}