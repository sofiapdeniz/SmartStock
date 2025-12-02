using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System;
using System.Collections.Generic;

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

        public List<PedidoVenda> GetPedidos()
        {
            return _pedidoRepository.GetPedidos();
        }

        public PedidoVenda PatchPedido(int id, PedidoVenda pedido)
        {
            return _pedidoRepository.PatchPedido(id, pedido);
        }

        public PedidoVenda PostPedido(PedidoVendaPostDTO dto)
        {
            if (dto == null)
                throw new Exception("O corpo da requisição é inválido.");

            var pedido = new PedidoVenda
            {
                ClienteNome = dto.ClienteNome,
                ValorTotal = dto.ValorTotal,
                EnderecoEntrega = dto.EnderecoEntrega,
                BairroEntrega = dto.BairroEntrega,
                NumeroEnderecoEntrega = dto.NumeroEnderecoEntrega,
                TelefoneCliente = dto.TelefoneCliente,
                TipoEntrega = dto.TipoEntrega,
                ItensPedido = new List<ItemPedido>()
            };

            foreach (var itemDto in dto.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                if (produto.Estoque < itemDto.Quantidade)
                    throw new Exception($"Estoque insuficiente do produto {produto.Nome}. Disponível: {produto.Estoque}");

                produto.Estoque -= itemDto.Quantidade;
                _produtoRepository.Update(produto);

                pedido.ItensPedido.Add(new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario
                });
            }

            return _pedidoRepository.PostPedido(pedido);
        }

        public PedidoVenda PutPedido(int id, PedidoVendaPutDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;

            pedido.ClienteNome = dto.ClienteNome;
            pedido.ValorTotal = dto.ValorTotal;
            pedido.TipoEntrega = dto.TipoEntrega;
            pedido.EnderecoEntrega = dto.EnderecoEntrega;
            pedido.BairroEntrega = dto.BairroEntrega;
            pedido.NumeroEnderecoEntrega = dto.NumeroEnderecoEntrega;
            pedido.TelefoneCliente = dto.TelefoneCliente;

            // Atualiza itens
            pedido.ItensPedido.Clear();
            foreach (var itemDto in dto.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                if (produto.Estoque < itemDto.Quantidade)
                    throw new Exception($"Estoque insuficiente do produto {produto.Nome}. Disponível: {produto.Estoque}");

                produto.Estoque -= itemDto.Quantidade;
                _produtoRepository.Update(produto);

                pedido.ItensPedido.Add(new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario
                });
            }

            return _pedidoRepository.PutPedido(id, pedido);
        }
    }
}
