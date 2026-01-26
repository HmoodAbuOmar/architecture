using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product requset);
        Task<List<Product>> GetAllAsync();
        Task<Product?> FindByIdAsync(int id);
        Task<bool> DecreaseQuantityAsync(List<(int productId, int quantity)> items);
        IQueryable<Product> Query();
    }
}
