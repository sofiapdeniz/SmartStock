using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System.Data;
using Microsoft.EntityFrameworkCore; // Necessário para acessar o IQueryable (se o GetById retornar IQueryable/Includes)

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

        // --- Métodos de Consulta e Atualização (Sem Alteração) ---
        
        public PedidoCompra Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            
            // Lógica de Negócios: Ao deletar um pedido de compra, o estoque deve ser subtraído.
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

        public PedidoCompra GetById(int id) => _pedidoRepository.GetById(id);

        public List<PedidoCompra> GetPedidos() => _pedidoRepository.GetPedidos();

        public PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            
            // Este método deveria ter lógica de ajuste de estoque semelhante ao PUT
            // caso o PATCH permita alteração de itens, mas vamos manter o foco no PUT.
            return _pedidoRepository.PatchPedido(id, dto); 
        }

        // ----------------------------------------------------------------

        public PedidoCompra PostPedido(PedidoCompraPostDTO newPedido)
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

                // LÓGICA DE ESTOQUE: Adiciona a quantidade comprada ao estoque
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

            // ATUALIZA ESTOQUE (Persiste as mudanças de estoque no banco)
            foreach (var produto in produtosParaAtualizar)
            {
                _produtoRepository.Update(produto); 
            }

            return pedidoSalvo;
        }

        // --- MÉTODO PUT CORRIGIDO PARA AJUSTAR ESTOQUE ---

        public PedidoCompra PutPedido(int id, PedidoCompraPutDTO dto)
        {
            // 1. CARREGAR O PEDIDO ANTIGO COM ITENS (Repositorio usa .Include())
            var pedidoAntigo = _pedidoRepository.GetById(id); 
            if (pedidoAntigo == null) return null;

            // Mapeamento de propriedades principais
            pedidoAntigo.NomeFornecedor = dto.NomeFornecedor;
            pedidoAntigo.CondicaoPagamento = dto.CondicaoPagamento;
            pedidoAntigo.Contato = dto.Contato;
            
            // Usamos um Dictionary para mapear os itens novos por ProdutoId para fácil acesso
            var novosItensMap = dto.Itens.ToDictionary(i => i.ProdutoId, i => i);

            // 2. PROCESSAR ITENS EXISTENTES: Calcular e aplicar a diferença de estoque
            foreach (var itemAntigo in pedidoAntigo.ItensPedido.ToList())
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
                            // APLICAÇÃO DA LÓGICA DE ESTOQUE: Adiciona ou subtrai a diferença
                            produto.Estoque += diferencaQuantidade; 
                            _produtoRepository.Update(produto);
                        }
                    }
                    
                    // ATUALIZA O ITEM NO PEDIDO
                    itemAntigo.Quantidade = itemNovoDto.Quantidade;
                    // Remove da lista de novos itens para saber quais foram ADICIONADOS depois
                    novosItensMap.Remove(itemAntigo.ProdutoId);
                }
                else
                {
                    // Item NÃO existe na nova lista (FOI REMOVIDO)
                    
                    var produto = _produtoRepository.GetById(itemAntigo.ProdutoId);
                    if (produto != null)
                    {
                        // LÓGICA DE ESTOQUE: Subtrai o item que foi removido do pedido
                        produto.Estoque -= itemAntigo.Quantidade;
                        _produtoRepository.Update(produto);
                    }
                    // Marca o item antigo para remoção (necessário no EF Core)
                    pedidoAntigo.ItensPedido.Remove(itemAntigo);
                }
            }
            
            // 3. PROCESSAR NOVOS ITENS: Itens que restaram no novosItensMap (FORAM ADICIONADOS)
            foreach (var itemNovoDto in novosItensMap.Values)
            {
                var produto = _produtoRepository.GetById(itemNovoDto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto com Id={itemNovoDto.ProdutoId} não encontrado.");

                // LÓGICA DE ESTOQUE: Adiciona a nova quantidade ao estoque
                produto.Estoque += itemNovoDto.Quantidade;
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

            // 4. Salva as mudanças do Pedido (cabeçalho, itens removidos, alterados e adicionados)
            return _pedidoRepository.PutPedido(id, dto);
        }
    }
}