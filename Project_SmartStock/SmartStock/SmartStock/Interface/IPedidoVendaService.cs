using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Interface
{
    public interface IPedidoVendaService
    {
        List<PedidoVenda> GetPedidos();
        PedidoVenda GetById(int id);
        PedidoVenda PostPedido(PedidoVendaPostDTO pedido);
        PedidoVenda PutPedido(int id, PedidoVenda pedido);
        PedidoVenda PatchPedido(int id, PedidoVenda pedido);
        PedidoVenda Delete(int id);
        
    }
}
