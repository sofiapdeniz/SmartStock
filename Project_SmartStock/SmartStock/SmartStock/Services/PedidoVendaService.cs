// EM PedidoVendaService.cs

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

        public PedidoVenda Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;

            // Lógica de Negócios: Ao deletar uma venda, o estoque deve ser reabastecido.
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
            // Implementação do Patch não fornecida, mantendo a original.
            return _pedidoRepository.PatchPedido(id, pedido);
        }

        public PedidoVenda PostPedido(PedidoVendaPostDTO dto)
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
                ItensPedido = new List<ItemPedido>()
            };

            foreach (var itemDto in dto.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                // Verifica estoque ANTES de subtrair
                if (produto.Estoque < itemDto.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente do produto {produto.Nome}. Disponível: {produto.Estoque}");

                // LÓGICA DE ESTOQUE: Subtrai a quantidade vendida
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

            // 1. Salva a Venda
            var pedidoSalvo = _pedidoRepository.PostPedido(pedido);

            // 2. ATUALIZA ESTOQUE (Persiste as mudanças de estoque no banco)
            foreach (var produto in produtosParaAtualizar)
            {
                _produtoRepository.Update(produto);
            }

            return pedidoSalvo;
        }


        // --- MÉTODO PUT CORRIGIDO PARA AJUSTAR ESTOQUE ---
        public PedidoVenda PutPedido(int id, PedidoVendaPutDTO dto)
        {
            // 1. CARREGAR O PEDIDO ANTIGO COM ITENS (Assume que o Repository usa Include)
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

            // Lista para itens que precisam ser removidos da coleção antiga
            var itensParaRemover = new List<ItemPedido>(); 

            // 2. PROCESSAR ITENS EXISTENTES: Calcular e aplicar a diferença de estoque
            foreach (var itemAntigo in pedidoAntigo.ItensPedido)
            {
                if (novosItensMap.TryGetValue(itemAntigo.ProdutoId, out var itemNovoDto))
                {
                    // Item existe na nova lista (FOI ALTERADO)

                    var diferencaQuantidade = itemNovoDto.Quantidade - itemAntigo.Quantidade;

                    if (diferencaQuantidade != 0)
                    {
                        var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                        if (produto != null)
                        {
                            // LÓGICA DE ESTOQUE DE VENDA:
                            // Se diferenca > 0 (vendeu mais): subtrai (estoque - diferenca)
                            // Se diferenca < 0 (vendeu menos): adiciona (estoque + |diferenca|)
                            produto.Estoque -= diferencaQuantidade;

                            // Verificação de estoque após o ajuste
                            if (produto.Estoque < 0)
                                throw new InvalidOperationException($"Ajuste falhou. Estoque insuficiente para o novo pedido do produto {produto.Nome}.");

                            _produtoRepository.Update(produto);
                        }
                    }

                    // ATUALIZA O ITEM NO PEDIDO
                    itemAntigo.Quantidade = itemNovoDto.Quantidade;
                    novosItensMap.Remove(itemAntigo.ProdutoId);
                }
                else
                {
                    // Item NÃO existe na nova lista (FOI REMOVIDO)

                    var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                    if (produto != null)
                    {
                        // LÓGICA DE ESTOQUE: Item removido da venda = Repõe o estoque
                        produto.Estoque += itemAntigo.Quantidade;
                        _produtoRepository.Update(produto);
                    }
                    // Adiciona o item à lista de remoção (para evitar modificar a coleção no loop)
                    itensParaRemover.Add(itemAntigo);
                }
            }
            
            // Remove itens fora do loop
            foreach(var item in itensParaRemover)
            {
                pedidoAntigo.ItensPedido.Remove(item);
            }

            // 3. PROCESSAR NOVOS ITENS: Itens que restaram no novosItensMap (FORAM ADICIONADOS)
            foreach (var itemNovoDto in novosItensMap.Values)
            {
                var produto = _produtoRepository.GetById(itemNovoDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemNovoDto.ProdutoId} não encontrado.");

                // LÓGICA DE ESTOQUE: Subtrai a nova quantidade do estoque
                if (produto.Estoque < itemNovoDto.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente para o novo item {produto.Nome}. Disponível: {produto.Estoque}");

                produto.Estoque -= itemNovoDto.Quantidade;
                _produtoRepository.Update(produto);

                // CRIA E ADICIONA NOVO ITEM
                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemNovoDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario,
                    UnidadeMedida = produto.UnidadeMedida
                };
                pedidoAntigo.ItensPedido.Add(itemPedido);
            }

            // 4. Salva as mudanças do Pedido
            return _pedidoRepository.PutPedido(id, pedidoAntigo);
        }
    }
}