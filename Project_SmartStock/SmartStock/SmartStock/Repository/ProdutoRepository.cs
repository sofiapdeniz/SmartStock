using SmartStock.Models;
using SmartStock.Data;
using SmartStock.Models.SmartStock.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace SmartStock.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DataContext _context;

        public ProdutoRepository(DataContext context)
        {
            _context = context;
        }

        public Produto Delete(int id)
        {
            var produto = _context.ProdutoTable.FirstOrDefault(f => f.Id == id);
            if (produto == null) return null;

            _context.ProdutoTable.Remove(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto GetById(int id) => _context.ProdutoTable
            .Include(p => p.Fornecedores) 
            .FirstOrDefault(f => f.Id == id);

        public List<Produto> GetProdutos() => _context.ProdutoTable
            .Include(p => p.Fornecedores) 
            .ToList();

        public Produto PatchProduto(int id, ProdutoPatchDTO dto)
        {
            var produto = _context.ProdutoTable.FirstOrDefault(f => f.Id == id);
            if (produto == null) return null;

            if (dto.Nome != null) produto.Nome = dto.Nome;
            if (dto.Codigo != null) produto.Codigo = dto.Codigo.Value;
            if (dto.Descricao != null) produto.Descricao = dto.Descricao;
            if (dto.PrecoUnitario != null) produto.PrecoUnitario = dto.PrecoUnitario.Value;
            if (dto.UnidadeMedida != null) produto.UnidadeMedida = dto.UnidadeMedida;

            produto.DataAtualizacao = DateTime.Now;
            _context.SaveChanges();
            return produto;
        }

        public Produto PostProduto(Produto produto)
        {
            _context.ProdutoTable.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto PutProduto(int id, ProdutoPutDTO dto)
        {
            var produto = _context.ProdutoTable.FirstOrDefault(f => f.Id == id);
            if (produto == null) return null;

            produto.Nome = dto.Nome;
            produto.Codigo = dto.Codigo;
            produto.Descricao = dto.Descricao;
            produto.PrecoUnitario = dto.PrecoUnitario;
            produto.UnidadeMedida = dto.UnidadeMedida;
            produto.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return produto;
        }

        public void Update(Produto produto)
        {
            _context.ProdutoTable.Update(produto);
            _context.SaveChanges();
        }
    }
}