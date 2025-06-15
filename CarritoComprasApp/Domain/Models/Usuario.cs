namespace CarritoCompras.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string UsuarioLogin { get; set; }
        public string Identificacion { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; } // "Administrador" o "Comprador"
    }
}
