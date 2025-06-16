using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Infraestructure.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task ActualizarAsync(Producto producto);
        Task<List<Producto>> ObtenerDisponiblesAsync();
    }
}
