using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProductsForAdmin();
        Task<PagintedResponse<ProductUserResponse>> GetAllProductsForUser(string lang = "en", int page = 1,
            int limit = 3,
            string? search = null,
            int? categotyId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            bool asc = true
            );
        Task<ProductUserDetails> GetAllProductsDetailsForUser(int id, string lang = "en");
    }
}
