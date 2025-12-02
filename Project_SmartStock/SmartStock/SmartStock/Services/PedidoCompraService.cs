using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;

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

        public PedidoCompra Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            return _pedidoRepository.Delete(id);
        }

        public PedidoCompra GetById(int id) => _pedidoRepository.GetById(id);

        public List<PedidoCompra> GetPedidos() => _pedidoRepository.GetPedidos();

        public PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;
            return _pedidoRepository.PatchPedido(id, dto);
        }

        public PedidoCompra PostPedido(PedidoCompraPostDTO newPedido)
        {
            if (newPedido == null)
                throw new Exception("O corpo da requisição é inválido.");

            // 1. Verifica se o fornecedor existe
            var fornecedor = _fornecedorRepository.GetById(newPedido.FornecedorId);
            if (fornecedor == null)
                throw new Exception($"Fornecedor com Id={newPedido.FornecedorId} não encontrado.");

            // 2. Cria o pedido
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

            // 3. Adiciona itens e atualiza o estoque
            foreach (var itemDto in newPedido.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                // Atualiza estoque
                produto.Estoque += itemDto.Quantidade;
                _produtoRepository.Update(produto);

                // Cria item do pedido
                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario
                };
                pedido.ItensPedido.Add(itemPedido);
            }

            // 4. Salva o pedido
            return _pedidoRepository.PostPedido(pedido);
        }

        public PedidoCompra PutPedido(int id, PedidoCompraPutDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return null;

            pedido.NomeFornecedor = dto.NomeFornecedor;
            pedido.CondicaoPagamento = dto.CondicaoPagamento;
            pedido.Contato = dto.Contato;
            pedido.ItensPedido = new List<ItemPedido>();

            foreach (var itemDto in dto.Itens)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");

                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoUnitario
                };
                pedido.ItensPedido.Add(itemPedido);
            }

            return _pedidoRepository.PutPedido(id, dto);
        }
    }
}
