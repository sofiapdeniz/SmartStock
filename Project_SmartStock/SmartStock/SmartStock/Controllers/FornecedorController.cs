using Microsoft.AspNetCore.Mvc;
using SmartStock.Interface;
using SmartStock.Models.SmartStock.Models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedorController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }
        // GET: api/<FornecedorController>
        [HttpGet]
        public IActionResult Get()
        {
            var fornecedores = _fornecedorService.GetFornecedores();
            return Ok(fornecedores);
        }

        // GET api/<FornecedorController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var fornecedores = _fornecedorService.GetById(id);

            if (fornecedores == null)
                return NotFound(new { Message = $"Fornecedor com o Id={id} não encontrado" });

            return Ok(fornecedores);
        }

        // POST api/<FornecedorController>
        [HttpPost]
        public IActionResult Post([FromBody] FornecedorPostDTO newFornecedor)
        {
            try
            {
                var fornecedor = _fornecedorService.PostFornecedor(newFornecedor);
                return CreatedAtAction(nameof(GetById), new { id = fornecedor.Id }, fornecedor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });

            }
        }
            // PUT api/<FornecedorController>/5
            [HttpPut("{id}")]
            public IActionResult Put(int id, [FromBody] FornecedorPutDTO dto)
            {
                var atualizado = _fornecedorService.PutFornecedor(id, dto);

                if (atualizado == null)
                    NotFound(new { Message = $"Fornecedor com o Id={id} não encontrado" });

                return Ok(atualizado);

            }

            [HttpPatch("{id}")]
            public IActionResult Patch(int id, [FromBody] FornecedorPatchDTO dto)
            {
                var atualizado = _fornecedorService.PatchFornecedor(id, dto);
                if (atualizado == null) return NotFound(new { Message = "Fornecedor não encontrado." });
                return Ok(atualizado);
            }

        // DELETE api/<FornecedorController>/5
        [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var fornecedores = _fornecedorService.Delete(id);

                if (fornecedores == null)
                    return NotFound(new { Message = $"Fornecedor com o Id={id} não encontrado" });

                return Ok(fornecedores);

            }
        }
    }


