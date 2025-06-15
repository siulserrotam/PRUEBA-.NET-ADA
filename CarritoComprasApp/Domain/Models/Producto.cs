namespace Domain.Models
{
    public class Producto
    {
        public int Id { get; set; } // Correcto
        public string Nombre { get; set; } = string.Empty; // Correcto
        public string Descripcion { get; set; } = string.Empty; // Correcto
        public int CantidadDisponible { get; set; } // Correcto

        // Navegación
        public ICollection<Transaccion>? Transacciones { get; set; } // Correcto
    }
}
