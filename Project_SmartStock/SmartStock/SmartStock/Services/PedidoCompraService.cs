// EM SmartStock.Services/PedidoCompraService.cs

using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System.Data;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using System;
using System.Collections.Generic;

namespace SmartStock.Services
{
    public class PedidoCompraService : IPedidoCompraService
    {
        private readonly IPedidoCompraRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public PedidoCompraService(
            IPedidoCompraRepository pedidoRepository, 
            IProdutoRepository produtoRepository,
            IFornecedorRepository fornecedorRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        // ----------------------------------------------------------------
        // MÉTODOS GET - AGORA RETORNAM DTOs DE RESPOSTA
        // ----------------------------------------------------------------

        public PedidoCompraResponseDTO GetById(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            return MapToResponseDTO(pedido); // Mapeia para o DTO antes de retornar
        }

        public List<PedidoCompraResponseDTO> GetPedidos()
        {
            var pedidos = _pedidoRepository.GetPedidos();
            return pedidos.Select(MapToResponseDTO).ToList(); 
        }

        // ----------------------------------------------------------------
        // MÉTODOS POST, PUT, PATCH E DELETE (Ajustados para DTO de Saída)
        // ----------------------------------------------------------------

        public PedidoCompraResponseDTO PostPedido(PedidoCompraPostDTO newPedido)
        {
            if (newPedido == null)
                throw new ArgumentException("O corpo da requisição é inválido.");

            var produtosParaAtualizar = new List<Produto>();

            var fornecedor = _fornecedorRepository.GetById(newPedido.FornecedorId);
            if (fornecedor == null)
                throw new KeyNotFoundException($"Fornecedor com Id={newPedido.FornecedorId} não encontrado.");

            var pedido = new PedidoCompra
            {
                FornecedorId = newPedido.FornecedorId,
                NomeFornecedor = fornecedor.Nome,
                CondicaoPagamento = newPedido.CondicaoPagamento,
                Contato = newPedido.Contato,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now,
                ItensPedido = new List<ItemPedido>()
            };

            foreach (var itemDto in newPedido.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                produto.Estoque += itemDto.Quantidade;
                produtosParaAtualizar.Add(produto);

                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario,
                    UnidadeMedida = produto.UnidadeMedida
                };
                
                pedido.ItensPedido.Add(itemPedido);
            }

            var pedidoSalvo = _pedidoRepository.PostPedido(pedido);

            foreach (var produto in produtosParaAtualizar)
            {
                _produtoRepository.Update(produto); 
            }

            var pedidoCompleto = _pedidoRepository.GetById(pedidoSalvo.Id); 
            return MapToResponseDTO(pedidoCompleto);
        }
        
        public PedidoCompraResponseDTO PutPedido(int id, PedidoCompraPutDTO dto)
        {
            var pedidoAntigo = _pedidoRepository.GetById(id); 
            if (pedidoAntigo == null) return null;

            pedidoAntigo.NomeFornecedor = dto.NomeFornecedor;
            pedidoAntigo.CondicaoPagamento = dto.CondicaoPagamento;
            pedidoAntigo.Contato = dto.Contato;
            
            var novosItensMap = dto.Itens.ToDictionary(i => i.ProdutoId, i => i);

            foreach (var itemAntigo in pedidoAntigo.ItensPedido.ToList())
            {
                if (novosItensMap.TryGetValue(itemAntigo.ProdutoId, out var itemNovoDto))
                {
                    var diferencaQuantidade = itemNovoDto.Quantidade - itemAntigo.Quantidade;
                    
                    if (diferencaQuantidade != 0)
                    {
                        var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                        if (produto != null)
                        {
                            produto.Estoque += diferencaQuantidade; 
                            _produtoRepository.Update(produto);
                        }
                    }
                    
                    itemAntigo.Quantidade = itemNovoDto.Quantidade;
                    novosItensMap.Remove(itemAntigo.ProdutoId);
                }
                else
                {
                    var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                    if (produto != null)
                    {
                        produto.Estoque -= itemAntigo.Quantidade;
                        _produtoRepository.Update(produto);
                    }
                    pedidoAntigo.ItensPedido.Remove(itemAntigo);
                }
            }
            
            foreach (var itemNovoDto in novosItensMap.Values)
            {
                var produto = _produtoRepository.GetById(itemNovoDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemNovoDto.ProdutoId} não encontrado.");

                produto.Estoque += itemNovoDto.Quantidade;
                _produtoRepository.Update(produto);
                
                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemNovoDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario,
                    UnidadeMedida = produto.UnidadeMedida
                };
                pedidoAntigo.ItensPedido.Add(itemPedido);
            }

            var pedidoAtualizado = _pedidoRepository.PutPedido(id, dto);
            var pedidoCompleto = _pedidoRepository.GetById(pedidoAtualizado.Id);
            return MapToResponseDTO(pedidoCompleto);
        }

        public PedidoCompraResponseDTO PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            
            var pedidoAtualizado = _pedidoRepository.PatchPedido(id, dto);
            return MapToResponseDTO(pedidoAtualizado);
        }

        public PedidoCompra Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            
            foreach (var item in pedido.ItensPedido)
            {
                var produto = _produtoRepository.GetById(item.ProdutoId);
                if (produto != null)
                {
                    produto.Estoque -= item.Quantidade;
                    _produtoRepository.Update(produto);
                }
            }
            
            return _pedidoRepository.Delete(id);
        }

        // ----------------------------------------------------------------
        // MÉTODO DE MAPEAMENTO (PRIVADO) - ATUALIZADO
        // ----------------------------------------------------------------

        private PedidoCompraResponseDTO MapToResponseDTO(PedidoCompra pedido)
        {
            if (pedido == null) return null;
            
            return new PedidoCompraResponseDTO
            {
                Id = pedido.Id,
                DataCriacao = pedido.DataCriacao,
                DataAtualizacao = pedido.DataAtualizacao,

                FornecedorId = pedido.FornecedorId,
                NomeFornecedor = pedido.NomeFornecedor, 
                Contato = pedido.Contato,
                CondicaoPagamento = pedido.CondicaoPagamento,
                
                Itens = pedido.ItensPedido?
                    .Select(item => new ItemPedidoResponseDTO
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        NomeProduto = item.Produto?.Nome, 
                        PrecoUnitario = item.PrecoUnitario,
                        Quantidade = item.Quantidade,
                        
                        // --- MAPEAMENTO DOS NOVOS CAMPOS ---
                        // Acessa as propriedades do objeto Produto que foi carregado
                        UnidadeMedida = item.Produto?.UnidadeMedida, 
                        Estoque = item.Produto != null ? item.Produto.Estoque : 0 // Proteção extra contra nulo
                        // ----------------------------------
                    })
                    .ToList()
                    ?? new List<ItemPedidoResponseDTO>()
            };
        }
    }
}