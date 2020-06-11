using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyProductAPI.Entites
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage ="A Data do pedido é obrigatória")]
        public DateTime Date { get; set;}
        [Required(ErrorMessage = "O Total é obrigatório")]

        [Column(TypeName = "decimal(9, 2)")]
        public decimal Total { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
