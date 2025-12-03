// EM SmartStock.Interface/IPedidoVendaService.cs

using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using System.Collections.Generic;

namespace SmartStock.Interface
{
    public interface IPedidoVendaService
    {
        List<PedidoVendaResponseDTO> GetPedidos();
        PedidoVendaResponseDTO GetById(int id);
        PedidoVendaResponseDTO PostPedido(PedidoVendaPostDTO pedido);
        PedidoVendaResponseDTO PutPedido(int id, PedidoVendaPutDTO pedido);
        PedidoVendaResponseDTO PatchPedido(int id, PedidoVenda pedido);
        
        PedidoVenda Delete(int id);
    }
}