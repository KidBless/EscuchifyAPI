using escuchify_api.Modelos;
using escuchify_api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace escuchify_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscosController : ControllerBase
{
    private readonly DiscoService _service;

    public DiscosController(DiscoService service)
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
        var disco = _service.ObtenerPorId(id);
        if (disco == null) return NotFound("Disco no encontrado");
        return Ok(disco);
    }

    [HttpGet("buscar-info")]
    public async Task<IActionResult> BuscarInfo([FromQuery] string titulo, [FromQuery] int artistaId)
    {
        if (string.IsNullOrWhiteSpace(titulo) || artistaId <= 0)
            return BadRequest("Faltan datos.");

        var info = await _service.BuscarDatosEnSpotify(titulo, artistaId);
        
        if (info == null) return NotFound("No se encontró en Spotify.");
        
        return Ok(info);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Disco disco)
    {
        try
        {
            await _service.CrearDisco(disco);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = disco.Id }, disco);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Actualizar(int id, [FromBody] Disco disco)
    {
        try
        {
            _service.ActualizarDisco(id, disco);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Disco no encontrado.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        _service.EliminarDisco(id);
        return NoContent();
    }
}