using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using System.Collections.Generic;

namespace SmartStock.Interface
{
    public interface IPedidoVendaService
    {
        List<PedidoVenda> GetPedidos();
        PedidoVenda GetById(int id);
        PedidoVenda PostPedido(PedidoVendaPostDTO pedido);       // DTO
        PedidoVenda PutPedido(int id, PedidoVendaPutDTO pedido); // DTO
        PedidoVenda PatchPedido(int id, PedidoVenda pedido);
        PedidoVenda Delete(int id);
    }
}
