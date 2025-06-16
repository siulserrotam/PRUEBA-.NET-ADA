using Application.Interfaces;
using Domain.Models;
using Infraestructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            return await _repo.ObtenerTodosAsync();
        }

        public async Task<Producto?> ObtenerProductoPorIdAsync(int id)
        {
            return await _repo.ObtenerPorIdAsync(id);
        }

        public async Task ActualizarProductoAsync(Producto producto)
        {
            await _repo.ActualizarAsync(producto);
        }

        public async Task<List<Producto>> ObtenerProductosDisponiblesAsync()
        {
            return await _repo.ObtenerDisponiblesAsync();
        }
    }
}
