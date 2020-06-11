using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyProductAPI.Entites
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="O Nome é obrigatório")]
        [MaxLength(50,ErrorMessage = "O Nome deve conter de 3 a 50 caracteres")]
        [MinLength(3,ErrorMessage = "O Nome deve conter de 3 a 50 caracteres")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "A Descrição deve conter no máximo 100 caracteres")]
        public string Descrition { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage ="A quantidade não pode ser negativa")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "O Preço é obrigatório ")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Column(TypeName = "decimal(9, 2)")] // outro exemplo de colocar tipo na variável está na classe BuyProductContext ;)
        public decimal Price { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
