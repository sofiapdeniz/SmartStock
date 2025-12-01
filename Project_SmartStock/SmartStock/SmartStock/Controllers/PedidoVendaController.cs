using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Data;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoVendaController : ControllerBase
    {
        private readonly IPedidoVendaService _pedidoService;

        public PedidoVendaController(IPedidoVendaService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/<Pedido>
        [HttpGet]
        public IActionResult Get()
        {
            var pedidos = _pedidoService.GetPedidos();
            return Ok(pedidos);
        }

        // GET api/<Pedido>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pedido = _pedidoService.GetById(id);

            if (pedido == null)
                return NotFound(new { Message = $"Pedido com o Id={id} não encontrado" });

            return Ok(pedido);
        }
        [HttpPost]
        public IActionResult Post([FromBody] PedidoCompraPostDTO newPedido)
        {
            try
            {
                var pedidoCriado = _pedidoService.PostPedido(newPedido);
                return CreatedAtAction(nameof(GetById), new { id = pedidoCriado.Id }, pedidoCriado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT api/<Pedido>/5
        [HttpPut("{id}")]
        public IActionResult PutPedido(int id, [FromBody] Models.PedidoVenda pedido)
        {
            var atualizado = _pedidoService.PutPedido(id, pedido);

            if (atualizado == null)
                return NotFound(new { Message = $"Pedido com o Id={id} não encontrado." });

            return Ok(atualizado);
        }

        // DELETE api/<Pedido>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pedido = _pedidoService.Delete(id);

            if (pedido == null)
                return NotFound(new { Message = $"Pedido com o Id={id} não encontrado." });

            return Ok(new { Message = "Pedido removido com sucesso." });
        }
    }
}
