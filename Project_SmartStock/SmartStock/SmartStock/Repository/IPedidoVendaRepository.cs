using SmartStock.Models;

namespace SmartStock.Repository
{
    public interface IPedidoVendaRepository
    {
        List<PedidoVenda> GetPedidos();
        PedidoVenda GetById(int id);
        PedidoVenda PostPedido(PedidoVenda pedido);
        PedidoVenda PutPedido(int id, PedidoVenda pedido);
        PedidoVenda PatchPedido(int id, PedidoVenda pedido);
        PedidoVenda Delete(int id);
    }
}
