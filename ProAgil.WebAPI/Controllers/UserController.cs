using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        
        public UserController(IConfiguration config,
                            UserManager<User> userManager
                            ,SignInManager<User> signInManager
                            , IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try{
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                var userToReturn  = _mapper.Map<UserDto>(user);

                if(result.Succeeded)
                {
                    return Created("GetUser", userToReturn);
                }

                return BadRequest (result.Errors);
            }
            catch(Exception ex)
            {
                 return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunica????o para gravar o usu??rio.({ex.Message})");
         
            }
            return BadRequest();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                // verifica se encontra um usu??rio com o userName
                var user =  await _userManager.FindByNameAsync(userLogin.UserName);
                
                if(user != null)
                {
                    // Checa se a senha do usu??rio encontrado ?? a mesma recebida pela API 
                    var result =  await _signInManager.CheckPasswordSignInAsync(user,userLogin.Password, false);
                    

                    if(result.Succeeded)
                    {   
                        var AppUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

                        var userToReturn = _mapper.Map<UserLoginDto>(AppUser);
                        // controle para gerar o token de autentica????o do usu??rio encontrado
                        return Ok(new {
                            token = GenerateJWToken(AppUser).Result,
                            user = userToReturn
                        });
                    }
                    
                }
                return Unauthorized("Login ou senha inv??lida");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu problemas na comunica????o para validar o login.({ex.Message})");
        
            }
        }

        private async Task<string> GenerateJWToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role , role));
            }
            
            // Busca a chave no config da aplica????o
            var key = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            // Gera a credencial usando a chave mais o tipo de algoritmo 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Cria o token limpo com as informa????es obtidas (claims, tempo de expira????o e chave)
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Cria o token criptografado com as informa????es do usu??rio
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // retorna o token do usu??rio
            return tokenHandler.WriteToken(token);
        }
    }
}