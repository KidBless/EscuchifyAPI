using escuchify_api.Modelos;
using escuchify_api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace escuchify_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistasController : ControllerBase
{
    private readonly ArtistaService _service;

    public ArtistasController(ArtistaService service)
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
    public IActionResult Crear([FromBody] Artista artista)
    {
        try
        {
            _service.RegistrarArtista(artista);
            // Retorna 201 Created y la ubicación del nuevo recurso
            return CreatedAtAction(nameof(ObtenerPorId), new { id = artista.Id }, artista);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // Error 400 por datos inválidos
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // Error 409 por conflicto (duplicado)
        }
    }

    [HttpPut("{id}")]
    public IActionResult Actualizar(int id, [FromBody] Artista artista)
    {
        try
        {
            _service.ActualizarArtista(id, artista);
            return NoContent(); // 204 Sin contenido (éxito)
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
}