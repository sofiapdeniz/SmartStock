using Microsoft.EntityFrameworkCore;
using SmartStock.Data;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;

namespace SmartStock.Services
{
    public class PedidoVendaService : IPedidoVendaService
    {
        private readonly IPedidoVendaRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public PedidoVendaService(IPedidoVendaRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }

        public PedidoVenda Delete(int id)
        {
            return _pedidoRepository.Delete(id);
        }

        public PedidoVenda GetById(int id)
        {
            return _pedidoRepository.GetById(id);
        }

        public List<PedidoVenda> GetPedidos(int id)
        {
            return _pedidoRepository.GetPedidos();
        }

        public List<PedidoVenda> GetPedidos()
        {
            throw new NotImplementedException();
        }

        public PedidoVenda PatchPedido(int id, PedidoVenda pedido)
        {
            return _pedidoRepository.PatchPedido(id, pedido);
        }

        public PedidoVenda PostPedido(PedidoVendaPostDTO newPedido)
        {
            if (newPedido == null)
                throw new Exception("o corpo da requisição é inválido.");

            if (_pedidoRepository.GetById(newPedido.Id) != null)
                throw new Exception($"Já existe um produto com o mesmo código={newPedido.Id}.");

            var pedido = new PedidoVenda
            {
                ClienteNome = newPedido.ClienteNome,
                ValorTotal = newPedido.ValorTotal,
                EnderecoEntrega = newPedido.EnderecoEntrega,
                BairroEntrega = newPedido.BairroEntrega,
                NumeroEnderecoEntrega = newPedido.NumeroEnderecoEntrega,
                TelefoneCliente = newPedido.TelefoneCliente,
                ItensPedido = new List<ItemPedido>()
            };
            foreach (var itemDto in newPedido.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                {
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");
                }
                var itemPedido = new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario,
                };
                pedido.ItensPedido.Add(itemPedido);
            }
            return _pedidoRepository.PostPedido(pedido);
        }

        public PedidoVenda PostPedido(PedidoCompraPostDTO pedido)
        {
            throw new NotImplementedException();
        }

        public PedidoVenda PutPedido(int id, PedidoVenda pedido)
        {
            return _pedidoRepository.PutPedido(id, pedido);
        }
    }
}
