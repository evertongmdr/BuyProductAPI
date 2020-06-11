using System;
namespace BuyProductAPI.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Descrition { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
        
}
