using BuyProductAPI.Entites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BuyProductAPI.Service.UserService.UserTypes;

namespace BuyProductAPI.Service.UserService
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid userId);
        IEnumerable<Order> GetOdersByUser(Guid userId);
        bool UserExists(Guid userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        User LoginUser(UserLoginProps user);
        Task<bool> EmailExists(string email);
        bool Save();

        // implementação de teste
        IEnumerable<User> GetUsers();

    }
}
