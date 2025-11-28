using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public class DiscoRepository : IDiscoRepository
{
    private readonly AppDbContext _context;

    public DiscoRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Disco> ObtenerTodos()
    {
        // Puedes agregar .Include(d => d.Artista) si quieres ver el artista en el listado
        return _context.Discos.ToList();
    }

    public Disco? ObtenerPorId(int id)
    {
        return _context.Discos
            .Include(d => d.Canciones) // Trae las canciones asociadas
            .Include(d => d.Artista)   // Trae el artista asociado
            .FirstOrDefault(d => d.Id == id);
    }

    public void Crear(Disco disco)
    {
        _context.Discos.Add(disco);
        _context.SaveChanges();
    }

    public void Actualizar(Disco disco)
    {
        _context.Discos.Update(disco);
        _context.SaveChanges();
    }

    public void Eliminar(Disco disco)
    {
        _context.Discos.Remove(disco);
        _context.SaveChanges();
    }
}