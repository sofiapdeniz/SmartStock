using SmartStock.Models;
using SmartStock.Data;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Repository
{
    public class PedidoCompraRepository : IPedidoCompraRepository
    {
        private readonly DataContext _context;

        public PedidoCompraRepository(DataContext context)
        {
            _context = context;
        }

        public PedidoCompra Delete(int id)
        {
            var pedido = _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);
            if (pedido == null)
                return null;

            _context.PedidoCompraTable.Remove(pedido);
            _context.SaveChanges();
            return pedido;

        }

        public PedidoCompra GetById(int id)
        {
            return _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);
        }

        public List<PedidoCompra> GetPedidos()
        {
            return _context.PedidoCompraTable.ToList();
        }

        public PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);

            if (pedido == null)
                return null;

            if (dto.NomeFornecedor != null)
                pedido.NomeFornecedor = dto.NomeFornecedor;

            if (dto.CondicaoPagamento.HasValue)
                pedido.CondicaoPagamento = dto.CondicaoPagamento.Value;

            if (dto.Contato != null)
                pedido.Contato = dto.Contato;

            pedido.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return pedido;
        }

        public PedidoCompra PostPedido(PedidoCompraPostDTO dto)
        {
            var pedido = new PedidoCompra
            {
                NomeFornecedor = dto.NomeFornecedor,
                CondicaoPagamento = dto.CondicaoPagamento,
                Contato = dto.Contato,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            _context.PedidoCompraTable.Add(pedido);
            _context.SaveChanges();
            return pedido;
        }

        public PedidoCompra PutPedido(int id, PedidoCompraPutDTO dto)
        {
            var pedido = _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);
            if (pedido == null)
                return null;

            pedido.NomeFornecedor = dto.NomeFornecedor;
            pedido.CondicaoPagamento = dto.CondicaoPagamento;
            pedido.Contato = dto.Contato;
            pedido.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return pedido;
        }
    }
}
