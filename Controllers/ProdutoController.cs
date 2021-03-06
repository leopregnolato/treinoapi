using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using treinoapi.Data;
using treinoapi.Models;
using treinoapi.HATEOAS;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace treinoapi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProdutoController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        private HATEOAS.HATEOAS HATEOAS;

        public ProdutoController(ApplicationDbContext database){
            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS("localhost:5001/api/v1/produto");
            HATEOAS.AddAction("GET_INFO","GET");
            HATEOAS.AddAction("DELETE_PRODUCT","DELETE");
            HATEOAS.AddAction("EDIT_PRODUCT","PATCH");   
        }

        [HttpGet("teste")]
        public IActionResult TesteClaims(){            
            return Ok(HttpContext.User.Claims.First(claim => claim.Type.ToString().Equals("Email", StringComparison.InvariantCultureIgnoreCase)).Value);
        }



        [HttpGet]
        public IActionResult Get(){
            var produtos = database.Produtos.ToList();
            List<ProdutoContainer> produtosHATEOAS = new List<ProdutoContainer>();
            foreach(var prod in produtos){
                ProdutoContainer produtoHATEOAS = new ProdutoContainer();
                produtoHATEOAS.produto = prod;
                produtoHATEOAS.links = HATEOAS.GetActions(prod.Id.ToString());
                produtosHATEOAS.Add(produtoHATEOAS);
            }
            return Ok(produtosHATEOAS);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){
            try{
                Produto produto = database.Produtos.First(p => p.Id == id);
                ProdutoContainer produtoHATEOAS = new ProdutoContainer();
                produtoHATEOAS.produto = produto;
                produtoHATEOAS.links = HATEOAS.GetActions(produto.Id.ToString());
                return Ok(produtoHATEOAS);
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("");
            }            
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoTemp pTemp){
            /* Valida????o */
            if(pTemp.Preco <= 0){
                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O pre??o do produto n??o pode ser menor ou igual a 0."});
            }

            if(pTemp.Nome.Length <= 1 ){
                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O nome do produto precisa ter mais de um caracter."});
            }

            Produto p = new Produto();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.Preco;
            database.Produtos.Add(p);
            database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("");
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] Produto produto){
            if(produto.Id > 0){
                try{
                    var p = database.Produtos.First(ptemp => ptemp.Id == produto.Id);
                    if(p != null){
                        p.Nome = produto.Nome != null ? produto.Nome : p.Nome;
                        p.Preco = produto.Preco != 0 ? produto.Preco : p.Preco;
                        database.SaveChanges();
                        return Ok();
                    }else{
                        Response.StatusCode = 400;
                        return new ObjectResult(new {msg = "Produto n??o encontrado."});
                    }
                }catch{
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Produto n??o encontrado."});                  
                }
            }else{
                Response.StatusCode = 400;
                return new ObjectResult(new {msg = "O Id do produto ?? inv??lido!"});
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            try{
                Produto produto = database.Produtos.First(p => p.Id == id);
                database.Produtos.Remove(produto);
                database.SaveChanges();
                return Ok();
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }

        public class ProdutoTemp{
            public string Nome { get; set; }
            public float Preco { get; set; }
        }

        public class ProdutoContainer{

            public Produto produto { get; set; }          
            public Link[] links { get; set; }
        }
        
    }
}