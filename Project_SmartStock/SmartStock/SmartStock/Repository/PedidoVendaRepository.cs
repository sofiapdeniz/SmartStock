using Microsoft.EntityFrameworkCore;
using SmartStock.Data;
using SmartStock.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartStock.Repository
{
    public class PedidoVendaRepository : IPedidoVendaRepository
    {
        private readonly DataContext _context;

        public PedidoVendaRepository(DataContext context)
        {
            _context = context;
        }

        public PedidoVenda Delete(int id)
        {
            var pedido = _context.PedidoVendaTable.FirstOrDefault(p => p.Id == id);
            if (pedido == null) return null;

            _context.PedidoVendaTable.Remove(pedido);
            _context.SaveChanges();
            return pedido;
        }

        public PedidoVenda GetById(int id)
        {
            return _context.PedidoVendaTable
                .Include(p => p.ItensPedido)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<PedidoVenda> GetPedidos()
        {
            return _context.PedidoVendaTable
                .Include(p => p.ItensPedido)
                .ToList();
        }

        public PedidoVenda PatchPedido(int id, PedidoVenda pedido)
        {
            throw new NotImplementedException();
        }

        public PedidoVenda PostPedido(PedidoVenda pedido)
        {
            _context.PedidoVendaTable.Add(pedido);
            _context.SaveChanges();
            return pedido;
        }

        public PedidoVenda PutPedido(int id, PedidoVenda pedido)
        {
            _context.PedidoVendaTable.Update(pedido);
            _context.SaveChanges();
            return pedido;
        }
    }
}
