using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using treinoapi.Data;
using treinoapi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;



namespace treinoapi.Controllers
{   
    [Route("api/v1/[controller]")]
    [ApiController]    
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        public UsuarioController(ApplicationDbContext database){
            this.database = database;
        }

        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario usuario){
            database.Add(usuario);
            database.SaveChanges();
            return Ok(new {msg = "Usuário cadastrado com sucesso!"});
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Usuario credenciais){
            try{
                Usuario usuario = database.Usuarios.First(user => user.Email.Equals(credenciais.Email));
                if(usuario != null){
                    if(usuario.Senha.Equals(credenciais.Senha)){
                        string chaveDeSeguranca = "gft_band_rock_n_roll_!";
                        var chaveSimetrica = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(chaveDeSeguranca));
                        var credenciaisDeAcesso = new SigningCredentials(chaveSimetrica,SecurityAlgorithms.HmacSha256Signature);
                        var JWT = new JwtSecurityToken(
                            issuer: "startergft.com",
                            expires: DateTime.Now.AddHours(1),
                            audience: "usuario_comum",
                            signingCredentials: credenciaisDeAcesso
                        );
                        return Ok(new JwtSecurityTokenHandler().WriteToken(JWT));
                    }else{
                        Response.StatusCode = 401;
                        return new ObjectResult("");
                    }
                }else{
                    Response.StatusCode = 401;
                    return new ObjectResult("");
                }
            }catch{
                Response.StatusCode = 401;
                return new ObjectResult("");
            }
        }

        private class Encoding
        {
        }
    }
}