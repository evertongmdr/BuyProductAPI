using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyProductAPI.Entites
{
    public class OrderProduct
    {
        public Guid OrderId { get; set;}
        public Guid ProductId { get; set;}
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

    }
}
