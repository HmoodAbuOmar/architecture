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
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepositry _cartRepositry;

        public CartService(IProductRepository productRepository, ICartRepositry cartRepositry)
        {
            _productRepository = productRepository;
            _cartRepositry = cartRepositry;
        }
        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _productRepository.FindByIdAsync(request.ProductId);

            if (product is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Product not found",
                };
            }

            if (product.Quantity < request.Count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Not Enough stock",
                };
            }

            var cartItem = await _cartRepositry.GetCartItemAsync(userId, request.ProductId);

            if (cartItem is not null)
            {
                cartItem.Count += request.Count;
                await _cartRepositry.UpdateAsync(cartItem);
            }
            else
            {
                var cart = request.Adapt<Cart>();
                cart.UserId = userId;
                await _cartRepositry.CreateAsync(cart);
            }

            return new BaseResponse
            {
                Success = true,
                Message = "Product added to cart successfully",
            };
        }
        public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
        {
            var cartItems = await _cartRepositry.GetUserCartAsync(userId);

            var items = cartItems.Select(c => new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,
                Count = c.Count,
                Price = c.Product.Price,
            }).ToList();

            //var response = cartItems.Adapt<CartSummaryResponse>();
            return new CartSummaryResponse
            {
                Items = items,
            };
        }

        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            await _cartRepositry.ClearCartAsync(userId);
            return new BaseResponse
            {
                Success = true,
                Message = "Cart cleared successfully",
            };
        }
    }
}
