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
            var cartItem = await _cartRepositry.GetCartItemAsync(userId, request.ProductId);

            var existingCount = cartItem?.Count ?? 0;

            if (product.Quantity < (existingCount + request.Count))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Not Enough stock",
                };
            }

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

            return new CartSummaryResponse
            {
                Items = items,
            };
        }

        public async Task<BaseResponse> UpdateQuantityAsync(string userId, int productId, int count)
        {
            var cartItem = await _cartRepositry.GetCartItemAsync(userId, productId);

            var product = await _productRepository.FindByIdAsync(productId);

            //if(count <= 0)
            //{
            //    return new BaseResponse
            //    {
            //        Success = false,
            //        Message = "Invalid Count",
            //    };
            //}

            if (count == 0)
            {
                await _cartRepositry.DeleteAsync(cartItem);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Product removed from cart successfully",
                };
            }

            if (product.Quantity < count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Not Enough stock",
                };
            }
            cartItem.Count = count;
            await _cartRepositry.UpdateAsync(cartItem);

            return new BaseResponse
            {
                Success = true,
                Message = "Cart item quantity updated successfully",
            };

        }



        public async Task<BaseResponse> RemoveFromCartAsync(string userId, int productId)
        {
            var cartItem = await _cartRepositry.GetCartItemAsync(userId, productId);

            if (cartItem is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cart item not found",
                };
            }

            await _cartRepositry.DeleteAsync(cartItem);

            return new BaseResponse
            {
                Success = true,
                Message = "Product removed from cart successfully",
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
