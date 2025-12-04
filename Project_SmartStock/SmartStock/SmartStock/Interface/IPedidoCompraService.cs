using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Interface
{
    public interface IPedidoCompraService
    {
        List<PedidoCompraResponseDTO> GetPedidos(); 
        PedidoCompraResponseDTO GetById(int id);
        PedidoCompraResponseDTO PostPedido(PedidoCompraPostDTO pedido);
        PedidoCompraResponseDTO PutPedido(int id, PedidoCompraPutDTO pedido);
        PedidoCompraResponseDTO PatchPedido(int id, PedidoCompraPatchDTO pedido);
        PedidoCompra Delete(int id);
    }
}