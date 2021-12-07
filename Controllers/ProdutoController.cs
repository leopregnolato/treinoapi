using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace treinoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        public IActionResult PegarProdutos(){
            return Ok(new {nome = "Leo Pregnolato", empresa = "GFT"});
        }

        [HttpGet("{id}")]
        public IActionResult PegarProdutos(int id){
            return Ok("Leo Pregnolato" + id);
        }
        
    }
}