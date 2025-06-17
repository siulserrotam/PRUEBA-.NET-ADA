namespace Domain.Models
{
    public class Transaccion

    {
        public int Id { get; set; } 
        public int UsuarioId { get; set; } 
        public int ProductoId { get; set; } 
        public int Cantidad { get; set; } 
        public DateTime Fecha { get; set; } = DateTime.Now; 

        public Usuario? Usuario { get; set; } 
        public Producto? Producto { get; set; } 
    }
}
