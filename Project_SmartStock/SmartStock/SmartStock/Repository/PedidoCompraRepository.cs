using SmartStock.Models;
using SmartStock.Data;
using SmartStock.Models.SmartStock.Models.DTOs;
using Microsoft.EntityFrameworkCore; // ESSENCIAL: Necessário para usar .Include() e DbUpdateException
using System; 
using System.Linq; // Necessário para FirstOrDefault e ToList

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
            if (pedido == null) return null;

            _context.PedidoCompraTable.Remove(pedido);
            _context.SaveChanges();
            return pedido;
        }

        // --- CORREÇÃO APLICADA: EAGER LOADING com .Include() e .ThenInclude() ---
        
        public PedidoCompra GetById(int id)
        {
            return _context.PedidoCompraTable
                .Include(pc => pc.Fornecedor) // Inclui o fornecedor
                .Include(pc => pc.ItensPedido) // Inclui a coleção de itens
                    .ThenInclude(ip => ip.Produto) // Para cada item, inclui o Produto (onde está o Estoque)
                .FirstOrDefault(pc => pc.Id == id);
        }

        public List<PedidoCompra> GetPedidos()
        {
            return _context.PedidoCompraTable
                .Include(pc => pc.Fornecedor) // Inclui o fornecedor
                .Include(pc => pc.ItensPedido) // Inclui a coleção de itens
                    .ThenInclude(ip => ip.Produto) // Para cada item, inclui o Produto (onde está o Estoque)
                .ToList();
        }
        
        // ------------------------------------------------------------------------

        public PedidoCompra PatchPedido(int id, PedidoCompraPatchDTO dto)
        {
            var pedido = _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);
            if (pedido == null) return null;

            if (dto.NomeFornecedor != null) pedido.NomeFornecedor = dto.NomeFornecedor;
            if (dto.CondicaoPagamento.HasValue) pedido.CondicaoPagamento = dto.CondicaoPagamento.Value;
            if (dto.Contato != null) pedido.Contato = dto.Contato;

            pedido.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
            return pedido;
        }

        // --- MÉTODO POST (MANTIDO) ---
        public PedidoCompra PostPedido(PedidoCompra pedido)
        {
            try
            {
                _context.PedidoCompraTable.Add(pedido);
                _context.SaveChanges();
                return pedido;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }
        // -----------------------------

        public PedidoCompra PutPedido(int id, PedidoCompraPutDTO dto)
        {
            var pedido = _context.PedidoCompraTable.FirstOrDefault(f => f.Id == id);
            if (pedido == null) return null;

            pedido.NomeFornecedor = dto.NomeFornecedor;
            pedido.CondicaoPagamento = dto.CondicaoPagamento;
            pedido.Contato = dto.Contato;
            pedido.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return pedido;
        }
    }
}