using escuchify_api.Modelos;
using escuchify_api.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace escuchify_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CancionesController : ControllerBase
{
    private readonly CancionService _service;

    public CancionesController(CancionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerPorId(int id)
    {
        var cancion = _service.ObtenerPorId(id);
        if (cancion == null) return NotFound("Canción no encontrada");
        return Ok(cancion);
    }

    [HttpPost]
    public IActionResult Crear([FromBody] Cancion cancion)
    {
        try
        {
            _service.CrearCancion(cancion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = cancion.Id }, cancion);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Actualizar(int id, [FromBody] Cancion cancion)
    {
        try
        {
            _service.ActualizarCancion(id, cancion);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Canción no encontrada.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        _service.EliminarCancion(id);
        return NoContent();
    }
}