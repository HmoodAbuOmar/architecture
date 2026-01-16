using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Repository;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepositry _cartRepository;

        public CheckoutService(ICartRepositry cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CheckoutResponse> ProcessPaymentAsync(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Cart is empty."
                };
            }

            decimal totalAmount = 0;
            foreach (var cart in cartItems)
            {
                if (cart.Product.Quantity < cart.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "not enough stock"
                    };
                }

                totalAmount += cart.Product.Price * cart.Count;
            }

            if (request.PaymentMethod == "cash")
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }

            else if (request.PaymentMethod == "visa")
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),

                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7237/checkout/success",
                    CancelUrl = $"https://localhost:7237/checkout/cancel",
                };


                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                            },
                            UnitAmount = (long)item.Product.Price,
                        },
                        Quantity = item.Count,
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);

                return new CheckoutResponse
                {
                    Success = true,
                    Url = session.Url,
                    PaymentId = session.Id,
                    Message = "Payment session created successfully."
                };
            }
            else
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Invalid payment method."
                };
            }

        }
    }
}
