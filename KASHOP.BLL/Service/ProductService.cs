using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileServices _fileServices;

        public ProductService(IProductRepository productRepository, IFileServices fileServices)
        {
            _productRepository = productRepository;
            _fileServices = fileServices;
        }
        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if (request.MainImage != null)
            {
                var imagePath = await _fileServices.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if (request.SupImages != null)
            {
                product.SupImages = new List<ProductImage>();
                foreach (var image in request.SupImages)
                {
                    var imagePath = await _fileServices.UploadAsync(image);
                    product.SupImages.Add(new ProductImage
                    {
                        ImageName = imagePath
                    });
                }

            }
            await _productRepository.AddAsync(product);

            return product.Adapt<ProductResponse>();
        }

        public async Task<PagintedResponse<ProductUserResponse>> GetAllProductsForUser(string lang = "en", int page = 1,
            int limit = 3,
            string? search = null,
            int? categotyId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            bool asc = true
            )
        {
            var query = _productRepository.Query();

            if (search is not null)
            {
                query = query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));
            }

            if (categotyId is not null)
            {
                query = query.Where(p => p.CategoryId == categotyId);
            }

            if (minPrice is not null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice is not null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }


            if (sortBy is not null)
            {
                sortBy = sortBy.ToLower();
                if (sortBy == "price")
                {
                    query = asc ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                }
                else if (sortBy == "name")
                {
                    query = asc ? query.OrderBy(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name)
                        : query.OrderByDescending(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name);
                }

                else if (sortBy == "rate")
                {
                    query = asc ? query.OrderBy(p => p.Rate) : query.OrderByDescending(p => p.Rate);
                }

            }

            var totalCount = await query.CountAsync();

            query = query.Skip((page - 1) * limit).Take(limit);


            var response = query.BuildAdapter()
                .AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();

            return new PagintedResponse<ProductUserResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = response
            };
        }


        public async Task<List<ProductResponse>> GetAllProductsForAdmin()
        {
            var products = await _productRepository.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }

        public async Task<ProductUserDetails> GetAllProductsDetailsForUser(int id, string lang = "en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var response = product.BuildAdapter()
              .AddParameters("lang", lang).AdaptToType<ProductUserDetails>();
            return response;
        }
    }
}
