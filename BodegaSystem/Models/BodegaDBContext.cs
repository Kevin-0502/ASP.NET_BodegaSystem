using Microsoft.EntityFrameworkCore;

namespace BodegaSystem.Models
{
    public class BodegaDBContext : DbContext
    {
        public BodegaDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<MovimientoInventario> Movimientos { get; set; }
    }
}
