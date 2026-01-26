using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Migrations;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IProductRepository _productRepository;

        public CheckoutService(ICartRepositry cartRepository, IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IOrderItemsRepository orderItemsRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemsRepository = orderItemsRepository;
            _productRepository = productRepository;
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


            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totalAmount,
            };


            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }

            else if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7237/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7237/checkout/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        { "UserId", userId },
                    }
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
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count,
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;
                await _orderRepository.CreateAsync(order);

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

        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];

            var order = await _orderRepository.GetBySessionIdAsync(sessionId);

            order.PaymentId = session.PaymentIntentId;

            order.OrderStatus = OrderStatusEnum.Approved;

            await _orderRepository.UpdateAsync(order);

            var user = await _userManager.FindByIdAsync(userId);

            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            var orderItems = new List<OrderItem>();

            var productUpdated = new List<(int productId, int quantity)>();
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    UnitPrice = cartItem.Product.Price,
                    Quantity = cartItem.Count,
                    TotalPrice = cartItem.Product.Price * cartItem.Count,
                };
                orderItems.Add(orderItem);
                productUpdated.Add((cartItem.ProductId, cartItem.Count));
                await _productRepository.DecreaseQuantityAsync(productUpdated);
            }
            await _orderItemsRepository.CreateRangeAsync(orderItems);
            await _cartRepository.ClearCartAsync(userId);
            await _emailSender.SendEmailAsync(user.Email, "Payment succesful", "<h2>thanks you </h2>");

            return new CheckoutResponse
            {
                Success = true,
                Message = "Payment completed succesfully"
            };
        }
    }
}
