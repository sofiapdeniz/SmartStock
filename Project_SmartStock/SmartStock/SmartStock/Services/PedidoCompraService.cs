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
        public PedidoCompraService(IPedidoCompraRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }
        public PedidoCompra Delete(int id)
        {
            var pedido = _pedidoRepository.GetById(id);

            if (pedido == null)
                return null;

            return _pedidoRepository.Delete(id);
        }

        public PedidoCompra GetById(int id)
        {
            return _pedidoRepository.GetById(id);
        }

        public List<PedidoCompra> GetPedidos()
        {
            return _pedidoRepository.GetPedidos();
        }

        public PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);

            if (pedido == null)

                return null;
            if (dto.NomeFornecedor != null)
                pedido.NomeFornecedor = dto.NomeFornecedor;

            if (dto.CondicaoPagamento.HasValue)
                pedido.CondicaoPagamento = dto.CondicaoPagamento.Value;

            return _pedidoRepository.PatchPedido(id, dto);

        }

        public PedidoCompra PostPedido(PedidoCompraPostDTO newPedido)
        { 
            if (newPedido == null)
                throw new Exception("o corpo da requisição é inválido.");

            if (_pedidoRepository.GetById(newPedido.Id) != null)
                throw new Exception($"Já existe um produto com o mesmo código={newPedido.Id}.");

        var pedido = new PedidoCompra
        {
            NomeFornecedor = newPedido.NomeFornecedor,
            CondicaoPagamento = newPedido.CondicaoPagamento,
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
            return _pedidoRepository.PostPedido(newPedido);
        }

        public PedidoCompra PutPedido(int id, PedidoCompraPutDTO dto)
        {
            var pedido = _pedidoRepository.GetById(id);

            if (pedido == null)
                return null;

            pedido.NomeFornecedor = dto.NomeFornecedor;
            pedido.CondicaoPagamento = dto.CondicaoPagamento;
            pedido.ItensPedido = new List<ItemPedido>();

            foreach (var itemDto in dto.Itens)
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
            return _pedidoRepository.PutPedido(id, dto);

        }
    }
}
