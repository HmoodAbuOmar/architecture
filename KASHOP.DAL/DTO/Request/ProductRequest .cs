using KASHOP.DAL.validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Request
{
    public class ProductRequest
    {
        public List<ProductTranslationRequest> Translations { get; set; }
        public decimal Price { get; set; }

        [MinmumValue(4)]
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> SupImages { get; set; }
        public int CategoryId { get; set; }
    }
}
