namespace CarritoCompras.Web.Models
{
    public class TransaccionDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}