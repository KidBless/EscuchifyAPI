using escuchify_api.Modelos;

namespace escuchify_api.Repositorios;

public class ArtistaRepository : IArtistaRepository
{
    private readonly AppDbContext _context;

    public ArtistaRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Artista> ObtenerTodos()
    {
        return _context.Artistas.ToList();
    }

    public Artista? ObtenerPorId(int id)
    {
        return _context.Artistas.Find(id);
    }

    public void Crear(Artista artista)
    {
        _context.Artistas.Add(artista);
        _context.SaveChanges();
    }

    public void Actualizar(Artista artista)
    {
        _context.Artistas.Update(artista);
        _context.SaveChanges();
    }

    public void Eliminar(Artista artista)
    {
        _context.Artistas.Remove(artista);
        _context.SaveChanges();
    }
}