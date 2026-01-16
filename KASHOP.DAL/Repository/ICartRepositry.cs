using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface ICartRepositry
    {
        Task<Cart> CreateAsync(Cart Request);

        Task<List<Cart>> GetUserCartAsync(String userId);

        Task<Cart?> GetCartItemAsync(string userId, int productId);

        Task<Cart> UpdateAsync(Cart Request);

        Task ClearCartAsync(string userId);
    }
}
