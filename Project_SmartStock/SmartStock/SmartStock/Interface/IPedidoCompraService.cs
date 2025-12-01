using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Interface
{
    public interface IPedidoCompraService
    {
        List<PedidoCompra> GetPedidos();
        PedidoCompra GetById(int id);
        PedidoCompra PostPedido(PedidoCompraPostDTO pedido);
        PedidoCompra PutPedido(int id, PedidoCompraPutDTO pedido);
        PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO pedido);
        PedidoCompra Delete(int id);

        
    }
}
