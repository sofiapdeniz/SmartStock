// EM SmartStock.Repository/FornecedorRepository.cs

using SmartStock.Models;
using SmartStock.Data;
using SmartStock.Models.SmartStock.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Adicionar se não estiver presente

namespace SmartStock.Repository
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly DataContext _context;

        public FornecedorRepository(DataContext context)
        {
            _context = context;
        }

        public Fornecedor GetById(int id)
        {
            // Mantido o .Include para carregar dados na resposta GET
            return _context.FornecedorTable
                .Include(f => f.ProdutosFornecidos)
                .ThenInclude(fp => fp.Produto)
                .FirstOrDefault(f => f.Id == id);
        }

        public List<Fornecedor> GetFornecedores()
        {
            // Mantido o .Include para carregar dados na resposta GET
            return _context.FornecedorTable
                .Include(f => f.ProdutosFornecidos)
                .ThenInclude(fp => fp.Produto)
                .ToList();
        }

        // CORREÇÃO: Recebe a Entidade completa, mapeada pelo Service
        public Fornecedor PostFornecedor(Fornecedor fornecedor) 
        {
            _context.FornecedorTable.Add(fornecedor);
            _context.SaveChanges();
            return fornecedor;
        }

        // CORREÇÃO: Recebe a Entidade e não o DTO para o PUT
        public Fornecedor PutFornecedor(Fornecedor fornecedor) 
        {
            _context.FornecedorTable.Update(fornecedor);
            _context.SaveChanges();
            return fornecedor;
        }

        // CORREÇÃO: Recebe a Entidade e não o DTO para o PATCH
        public Fornecedor PatchFornecedor(Fornecedor fornecedor) 
        {
            _context.FornecedorTable.Update(fornecedor);
            _context.SaveChanges();
            return fornecedor;
        }

        public Fornecedor Delete(int id)
        {
            var fornecedor = _context.FornecedorTable.FirstOrDefault(f => f.Id == id);
            if (fornecedor == null) return null;

            _context.FornecedorTable.Remove(fornecedor);
            _context.SaveChanges();
            return fornecedor;
        }
    }
}