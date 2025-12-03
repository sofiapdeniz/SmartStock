// EM SmartStock.Interface/IPedidoCompraService.cs

using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Interface
{
    public interface IPedidoCompraService
    {
        // Alterado o retorno para o DTO de Response
        List<PedidoCompraResponseDTO> GetPedidos(); 
        PedidoCompraResponseDTO GetById(int id);
        
        // Alterado o retorno do POST, PUT, PATCH para o DTO de Response
        PedidoCompraResponseDTO PostPedido(PedidoCompraPostDTO pedido);
        PedidoCompraResponseDTO PutPedido(int id, PedidoCompraPutDTO pedido);
        PedidoCompraResponseDTO PatchPedido(int id, PedidoCompraPatchDTO pedido);
        
        PedidoCompra Delete(int id);
    }
}