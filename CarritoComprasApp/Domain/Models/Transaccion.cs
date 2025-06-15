namespace Domain.Models
{
    public class Transaccion
    {
        public int Id { get; set; } // Correcto
        public int UsuarioId { get; set; } // Correcto
        public int ProductoId { get; set; } // Correcto
        public int Cantidad { get; set; } // Correcto
        public DateTime Fecha { get; set; } = DateTime.Now; // Correcto

        // Navegación
        public Usuario? Usuario { get; set; } // Correcto
        public Producto? Producto { get; set; } // Correcto
    }
}
