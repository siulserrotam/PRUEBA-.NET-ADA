using Domain.Models;

namespace Application.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerProductosAsync();
        Task<Producto?> ObtenerProductoPorIdAsync(int id);
        Task ActualizarProductoAsync(Producto producto);
        Task<List<Producto>> ObtenerProductosDisponiblesAsync(); 
    }
}
