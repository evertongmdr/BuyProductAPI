using System.ComponentModel.DataAnnotations;

namespace BuyProductAPI.Service.UserService
{
    public class UserTypes
    {
        public struct UserLoginProps
        {
            [Required(ErrorMessage = "Email obrigatório")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Senha obrigatória")]
            public string Password { get; set; }
        }
    }


}
