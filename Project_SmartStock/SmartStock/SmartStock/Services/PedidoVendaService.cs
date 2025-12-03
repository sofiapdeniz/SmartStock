// EM SmartStock.Services/PedidoVendaService.cs

using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System;
using System.Collections.Generic;
using System.Linq; 

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

        // ----------------------------------------------------------------
        // MÉTODOS GET - AGORA RETORNAM DTOs DE RESPOSTA
        // ----------------------------------------------------------------

        public PedidoVendaResponseDTO GetById(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            return MapToResponseDTO(pedido);
        }

        public List<PedidoVendaResponseDTO> GetPedidos()
        {
            var pedidos = _pedidoRepository.GetPedidos();
            return pedidos.Select(MapToResponseDTO).ToList();
        }

        // ----------------------------------------------------------------
        // MÉTODOS POST, PUT, PATCH - AGORA RETORNAM DTOs DE RESPOSTA
        // ----------------------------------------------------------------

        public PedidoVendaResponseDTO PostPedido(PedidoVendaPostDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("O corpo da requisição é inválido.");

            var produtosParaAtualizar = new List<Produto>();

            var pedido = new PedidoVenda
            {
                ClienteNome = dto.ClienteNome,
                ValorTotal = dto.ValorTotal,
                EnderecoEntrega = dto.EnderecoEntrega,
                BairroEntrega = dto.BairroEntrega,
                NumeroEnderecoEntrega = dto.NumeroEnderecoEntrega,
                TelefoneCliente = dto.TelefoneCliente,
                TipoEntrega = dto.TipoEntrega,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now,
                LojaRetirada = dto.LojaRetirada,
                ItensPedido = new List<ItemPedido>()
            };

            foreach (var itemDto in dto.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                if (produto.Estoque < itemDto.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente do produto {produto.Nome}. Disponível: {produto.Estoque}");

                produto.Estoque -= itemDto.Quantidade;
                produtosParaAtualizar.Add(produto); 

                pedido.ItensPedido.Add(new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario,
                    UnidadeMedida = produto.UnidadeMedida 
                });
            }

            var pedidoSalvo = _pedidoRepository.PostPedido(pedido);

            foreach (var produto in produtosParaAtualizar)
            {
                _produtoRepository.Update(produto);
            }
            
            // Recarrega para garantir que as relações (Produto) estejam carregadas para o DTO
            var pedidoCompleto = _pedidoRepository.GetById(pedidoSalvo.Id);
            return MapToResponseDTO(pedidoCompleto);
        }


        public PedidoVendaResponseDTO PutPedido(int id, PedidoVendaPutDTO dto)
        {
            var pedidoAntigo = _pedidoRepository.GetById(id);
            if (pedidoAntigo == null) return null;

            // Mapeamento de propriedades principais
            pedidoAntigo.ClienteNome = dto.ClienteNome;
            pedidoAntigo.ValorTotal = dto.ValorTotal;
            pedidoAntigo.TipoEntrega = dto.TipoEntrega;
            pedidoAntigo.EnderecoEntrega = dto.EnderecoEntrega;
            pedidoAntigo.BairroEntrega = dto.BairroEntrega;
            pedidoAntigo.NumeroEnderecoEntrega = dto.NumeroEnderecoEntrega;
            pedidoAntigo.TelefoneCliente = dto.TelefoneCliente;
            pedidoAntigo.DataAtualizacao = DateTime.Now;

            var novosItensMap = dto.Itens.ToDictionary(i => i.ProdutoId, i => i);
            var itensParaRemover = new List<ItemPedido>(); 

            // 2. PROCESSAR ITENS EXISTENTES
            foreach (var itemAntigo in pedidoAntigo.ItensPedido)
            {
                if (novosItensMap.TryGetValue(itemAntigo.ProdutoId, out var itemNovoDto))
                {
                    var diferencaQuantidade = itemNovoDto.Quantidade - itemAntigo.Quantidade;

                    if (diferencaQuantidade != 0)
                    {
                        var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                        if (produto != null)
                        {
                            produto.Estoque -= diferencaQuantidade; 

                            if (produto.Estoque < 0)
                                throw new InvalidOperationException($"Ajuste falhou. Estoque insuficiente para o novo pedido do produto {produto.Nome}.");

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
                        produto.Estoque += itemAntigo.Quantidade;
                        _produtoRepository.Update(produto);
                    }
                    itensParaRemover.Add(itemAntigo);
                }
            }
            
            foreach(var item in itensParaRemover)
            {
                pedidoAntigo.ItensPedido.Remove(item);
            }

            // 3. PROCESSAR NOVOS ITENS
            foreach (var itemNovoDto in novosItensMap.Values)
            {
                var produto = _produtoRepository.GetById(itemNovoDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemNovoDto.ProdutoId} não encontrado.");

                if (produto.Estoque < itemNovoDto.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente para o novo item {produto.Nome}. Disponível: {produto.Estoque}");

                produto.Estoque -= itemNovoDto.Quantidade;
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

            var pedidoAtualizado = _pedidoRepository.PutPedido(id, pedidoAntigo);
            
            // Recarrega para garantir que as relações (Produto) estejam carregadas para o DTO
            var pedidoCompleto = _pedidoRepository.GetById(pedidoAtualizado.Id);
            return MapToResponseDTO(pedidoCompleto);
        }
        
        public PedidoVendaResponseDTO PatchPedido(int id, PedidoVenda pedido)
        {
            var pedidoAtualizado = _pedidoRepository.PatchPedido(id, pedido);
            return MapToResponseDTO(pedidoAtualizado);
        }

        // ----------------------------------------------------------------
        // MÉTODO DELETE (Mantém o retorno da Entidade)
        // ----------------------------------------------------------------

        public PedidoVenda Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;

            foreach (var item in pedido.ItensPedido)
            {
                var produto = _produtoRepository.GetById(item.ProdutoId);
                if (produto != null)
                {
                    produto.Estoque += item.Quantidade;
                    _produtoRepository.Update(produto);
                }
            }

            return _pedidoRepository.Delete(id);
        }
        
        // ----------------------------------------------------------------
        // MÉTODO DE MAPEAMENTO (PRIVADO)
        // ----------------------------------------------------------------

        private PedidoVendaResponseDTO MapToResponseDTO(PedidoVenda pedido)
        {
            if (pedido == null) return null;
            
            return new PedidoVendaResponseDTO
            {
                Id = pedido.Id,
                DataCriacao = pedido.DataCriacao,
                DataAtualizacao = pedido.DataAtualizacao,
                ValorTotal = pedido.ValorTotal,
                
                ClienteNome = pedido.ClienteNome,
                TelefoneCliente = pedido.TelefoneCliente,

                // Converte o Enum para int para simplificar o DTO
                TipoEntrega = (int)pedido.TipoEntrega,
                EnderecoEntrega = pedido.EnderecoEntrega,
                BairroEntrega = pedido.BairroEntrega,
                NumeroEnderecoEntrega = pedido.NumeroEnderecoEntrega,
                LojaRetirada = pedido.LojaRetirada,
                
                Itens = pedido.ItensPedido?
                    .Select(item => new ItemVendaResponseDTO
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        // Mapeia os dados do produto que foi carregado
                        NomeProduto = item.Produto?.Nome, 
                        PrecoUnitario = item.PrecoUnitario,
                        Quantidade = item.Quantidade,
                        UnidadeMedida = item.Produto?.UnidadeMedida,
                        EstoqueAtual = item.Produto != null ? item.Produto.Estoque : 0
                    })
                    .ToList()
                    ?? new List<ItemVendaResponseDTO>()
            };
        }
    }
}