using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Data;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Services;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET: api/<Produto>
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _produtoService.GetProdutos();
            return Ok(produtos);
        }

        // GET api/<Produto>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _produtoService.GetById(id);

            if (produto == null)
                return NotFound(new { Message = $"Produto com o Id={id} não encontrado" });
           
            return Ok(produto);
        }

        // POST api/<Produto>
        [HttpPost]
        public IActionResult Post([FromBody] ProdutoPostDTO newProduto)
        {
            try
            {
                var produtoCriado = _produtoService.PostProduto(newProduto);
                return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);

            } catch (Exception ex) 
            {
                return BadRequest(new { Message = "Erro ao criar o produto.", Details = ex.Message });
            }
        }

        // PUT api/<Produto>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProdutoPutDTO produto)
        {
            var atualizado = _produtoService.PutProduto(id, produto);

            if (atualizado == null)
                return NotFound(new { Message = $"Produto com o Id={id} não encontrado." });

            return Ok(atualizado);
        }

        // PATCH api/<Produto>/5/complete
        //[HttpPatch("{id}/complete")]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] ProdutoPatchDTO dto)
        {
            var atualizado = _produtoService.PatchProduto(id, dto);
            if (atualizado == null) return NotFound(new { Message = "Produto não encontrado." });
            return Ok(atualizado);
        }


        // DELETE api/<Produto>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pedido = _produtoService.Delete(id);

            if (pedido == null)
                return NotFound(new { Message = $"Produto com o Id={id} não encontrado." });

            return Ok(new { Message = "Produto removido com sucesso." });
        }
    }
}
