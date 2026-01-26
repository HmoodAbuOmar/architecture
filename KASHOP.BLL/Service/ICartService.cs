using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(String userId, AddToCartRequest request);

        Task<CartSummaryResponse> GetUserCartAsync(String userId, string lang = "en");

        Task<BaseResponse> ClearCartAsync(string userId);
    }
}
