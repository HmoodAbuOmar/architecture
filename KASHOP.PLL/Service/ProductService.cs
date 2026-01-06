using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
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
        public async Task<List<ProductResponse>> GetAllProductsForAdmin()
        {
            var products = await _productRepository.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }
    }
}
