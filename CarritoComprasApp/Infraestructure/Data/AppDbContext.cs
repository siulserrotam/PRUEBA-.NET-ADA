using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets para las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Transacciones)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Transacciones)
                .WithOne(t => t.Producto)
                .HasForeignKey(t => t.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Si tienes más configuraciones personalizadas, agrégalas aquí
        }
    }
}
