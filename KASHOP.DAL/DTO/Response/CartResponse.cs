using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class CartResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; } = 1;
        public decimal Price { get; set; }

        public decimal TotalPrice => Price * Count;
    }
}
