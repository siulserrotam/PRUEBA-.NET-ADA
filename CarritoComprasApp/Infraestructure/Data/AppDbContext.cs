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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Transaccion)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Transaccion)
                .WithOne(t => t.Producto)
                .HasForeignKey(t => t.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Producto>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaccion>().HasKey(t => t.Id);

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
