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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<List<OrderResponse>> GetOrdersAsync(OrderStatusEnum status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);

            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            order.OrderStatus = newStatus;

            if (newStatus == OrderStatusEnum.Delivered)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
            }

            //else if (newStatus == OrderStatusEnum.Cancelled)
            //{
            //    if(order.OrderStatus == OrderStatusEnum.Shipped)
            //    {
            //        return new BaseResponse
            //        {
            //            Success = false,
            //            Message = "Cannot cancel an order that has already been shipped."
            //        };
            //    }

            await _orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Success = true,
                Message = "Order status updated successfully."
            };

        }


    }
}
