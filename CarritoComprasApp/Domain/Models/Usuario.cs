namespace Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty; // Codificada
        public string Rol { get; set; } = "Cliente"; // o "Administrador"

        // Navegación
        public ICollection<Transaccion>? Transaccion { get; set; }
    }
}
