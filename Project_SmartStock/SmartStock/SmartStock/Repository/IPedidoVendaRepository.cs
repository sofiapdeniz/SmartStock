using SmartStock.Models;
using System.Collections.Generic;

namespace SmartStock.Repository
{
    public interface IPedidoVendaRepository
    {
        List<PedidoVenda> GetPedidos();
        PedidoVenda GetById(int id);
        PedidoVenda PostPedido(PedidoVenda pedido);       // model
        PedidoVenda PutPedido(int id, PedidoVenda pedido); // model
        PedidoVenda PatchPedido(int id, PedidoVenda pedido);
        PedidoVenda Delete(int id);
    }
}
