using BuyProductAPI.Entites;
using BuyProductAPI.Models;
using BuyProductAPI.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using static BuyProductAPI.Service.UserService.UserTypes;

namespace BuyProductAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
        }

        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            //if (!_userRepository.UserExists(userId))
            //    return NotFound();

            var user = await _userRepository.GetUser(userId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        /*
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult LoginUser([FromBody] UserLoginProps user)
        {
            var userEntity = _userRepository.LoginUser(user);

            if (userEntity == null)
                return NotFound();

             return Ok(userEntity);

        
        }
        */
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult LoginUser([FromBody] UserLoginProps user)
        {
            var userEntity = _userRepository.LoginUser(user);

            if (userEntity == null)
                return NotFound();

            var token = GenerateToken();
           return Ok(new {
            userEntity,
            token = token
           });


        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody] User user)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.CreateUser(user))
            {
                ModelState.AddModelError("", $" Ocorreu um erro ao salvar usuario {user.FirstName} {user.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetUser", new { userId = user.Id }, user);
        }

        public string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /*
        [AcceptVerbs("Get","Post")]
        [Route("email-exists")]
        public async Task<IActionResult> EmailExists(string email)
        {

            var emailExists = await _userRepository.EmailExists(email);

            if (emailExists)
                return Ok(false);

            return Ok(true);
        }
        */

    }
}
