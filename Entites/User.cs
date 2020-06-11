using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuyProductAPI.Entites
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(50, ErrorMessage = "O nome deve conter entre 3 a 50 caracteres")]
        [MinLength(3, ErrorMessage = "o nome deve conter entre 3 a 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [MaxLength(50, ErrorMessage = "O sobrenome deve conter entre 3 a 50 caracteres")]
        [MinLength(3, ErrorMessage = "O sobrenome deve conter entre 3 a 50 caracteres")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage ="O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage ="E-mail inváido")]
        //[Remote(action: "EmailExists", controller: "Users", ErrorMessage ="Email já existe")]
        public string Email { get; set; }

        [Required(ErrorMessage ="A senha é obrigatória")]
        public string Password { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
