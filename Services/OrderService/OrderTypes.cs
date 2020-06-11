using BuyProductAPI.utilitarys.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyProductAPI.Services.OrderService
{
    public class OrderTypes
    {
        public struct ProductProps
        {
            [Required(ErrorMessage = "O atributo Id do produto é obrigatório")] // não funciona com Guid :(
            public Guid Id { get; set; }

            [Required(ErrorMessage = "O atributo quantidade do produto é obrigatório")]
            public int Quantity { get; set; }
            public decimal? price { get; set; }
        }
        public struct InputOrderCreating
        {
            [Required(ErrorMessage = "O atributo Id do usuário é obrigatório")]
            public Guid? userId { get; set; } // motivo que required não funciona com tipo Guid para bular isso usa o ? https://stackoverflow.com/questions/7187576/validation-of-guid

            [Required(ErrorMessage = "O atributo ProductsAddedOrder é obrigatório")]
            [NotEmptyArray]
            public ICollection<ProductProps> ProductsAddedOrder { get; set; }

        }
    }
}
