using BuyProductAPI.Contexts;
using BuyProductAPI.Entites;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BuyProductAPI.Service.UserService.UserTypes;

namespace BuyProductAPI.Service.UserService
{
    public class UserRepository : IUserRepository
    {
        private readonly BuyProductContext _userContext;
        public UserRepository(BuyProductContext userContext)
        {
            _userContext = userContext;
        }

        public bool CreateUser(User user)
        {
            _userContext.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            _userContext.Update(user);

            return Save();
        }
        public async Task<User> GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            return await _userContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public IEnumerable<Order> GetOdersByUser(Guid userId)
        {
            return _userContext.Oders.AsNoTracking().Where(o => o.UserId == userId).ToList();
        }

        public bool UserExists(Guid userId)
        {
            return _userContext.Users.AsNoTracking().Any(a => a.Id == userId);
        }

        /*
        public bool Save()
        {
           int saved = _userContext.SaveChanges();
           return saved >= 0 ? true : false;
        }
       */

        public bool Save()
        {

            try
            {
                int saved = _userContext.SaveChanges();

                return saved >= 0 ? true : false;

                /*
                 Talvez da para melhroar isso usando Validon Result e padronização de erros e outras lógicas
                Materiais que podem auxiliar
                https://www.wellingtonjhn.com/posts/padroniza%C3%A7%C3%A3o-de-respostas-de-erro-em-apis-com-problem-details/ 
                https://www.thereformedprogrammer.net/entity-framework-core-validating-data-and-catching-sql-errors/
                 
                 */
            }
            catch (DbUpdateException dbUpdateEx)
            {

                var sqlException = dbUpdateEx.InnerException as SqlException;

                if (sqlException != null && sqlException.Number == 2601)
                {
                    throw new Exception("Esse e-mail já existe", sqlException);

                }
                else
                {
                    throw dbUpdateEx;
                }
            }
        }

        // implementação de testes

        public IEnumerable<User> GetUsers()
        {
            return _userContext.Users.AsNoTracking();
        }

        public User LoginUser(UserLoginProps user)
        {
            return _userContext.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
        }

        public async Task<bool> EmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            return await _userContext.Users.AsNoTracking().AnyAsync(a => a.Email.ToLower() == email.ToLower());
        }
    }
}
