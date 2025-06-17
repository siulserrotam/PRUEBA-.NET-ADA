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

            // Configuración de la relación Usuario - Transacciones (1 a muchos)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Transaccion)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la relación Producto - Transacciones (1 a muchos)
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Transaccion)
                .WithOne(t => t.Producto)
                .HasForeignKey(t => t.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Opcional: establecer claves primarias si no se usan convenciones
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Producto>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaccion>().HasKey(t => t.Id);

            // Opcional: configuraciones adicionales como longitudes, requeridos, etc.
            modelBuilder.Entity<Usuario>()
                .Property(u => u.UsuarioLogin)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Clave)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Fecha)
                .HasColumnType("datetime");
        }
    }
}
