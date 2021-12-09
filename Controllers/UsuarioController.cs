using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using treinoapi.Data;
using treinoapi.Models;

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
                        return Ok("logado");
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
    }
}