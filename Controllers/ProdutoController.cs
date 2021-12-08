using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using treinoapi.Data;
using treinoapi.Models;

namespace treinoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        public ProdutoController(ApplicationDbContext database){
            this.database = database;
        }

        [HttpGet]
        public IActionResult PegarProdutos(){
            return Ok(new {nome = "Leo Pregnolato", empresa = "GFT"});
        }

        [HttpGet("{id}")]
        public IActionResult PegarProdutos(int id){
            return Ok("Leo Pregnolato" + id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoTemp pTemp){
            Produto p = new Produto();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.Preco;
            database.Produtos.Add(p);
            database.SaveChanges();

            return Ok(new {msg = "Você criou um novo produto:"});
        }

        public class ProdutoTemp{
            public string Nome { get; set; }
            public float Preco { get; set; }
        }
        
    }
}