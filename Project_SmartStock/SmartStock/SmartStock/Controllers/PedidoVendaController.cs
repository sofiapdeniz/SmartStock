// EM SmartStock.Controllers/PedidoVendaController.cs

using Microsoft.AspNetCore.Mvc;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using System;

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

        [HttpGet]
        public IActionResult Get()
        {
            // O serviço retorna List<PedidoVendaResponseDTO>
            var pedidos = _pedidoService.GetPedidos();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // O serviço retorna PedidoVendaResponseDTO
            var pedido = _pedidoService.GetById(id);
            if (pedido == null)
                return NotFound(new { Message = $"Pedido com o Id={id} não encontrado" });

            return Ok(pedido);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PedidoVendaPostDTO newPedido)
        {
            try
            {
                // O serviço retorna PedidoVendaResponseDTO
                var pedidoCriado = _pedidoService.PostPedido(newPedido);
                return CreatedAtAction(nameof(GetById), new { id = pedidoCriado.Id }, pedidoCriado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutPedido(int id, [FromBody] PedidoVendaPutDTO dto)
        {
            try
            {
                // O serviço retorna PedidoVendaResponseDTO
                var atualizado = _pedidoService.PutPedido(id, dto);
                if (atualizado == null)
                    return NotFound(new { Message = $"Pedido com o Id={id} não encontrado." });

                return Ok(atualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

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