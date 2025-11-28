using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public class CancionRepository : ICancionRepository
{
    private readonly AppDbContext _context;

    public CancionRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Cancion> ObtenerTodas()
    {
        return _context.Canciones.ToList();
    }

    public Cancion? ObtenerPorId(int id)
    {
        return _context.Canciones
            .Include(c => c.Disco) // Opcional: ver a qué disco pertenece
            .FirstOrDefault(c => c.Id == id);
    }

    public void Crear(Cancion cancion)
    {
        _context.Canciones.Add(cancion);
        _context.SaveChanges();
    }

    public void Actualizar(Cancion cancion)
    {
        _context.Canciones.Update(cancion);
        _context.SaveChanges();
    }

    public void Eliminar(Cancion cancion)
    {
        _context.Canciones.Remove(cancion);
        _context.SaveChanges();
    }
}