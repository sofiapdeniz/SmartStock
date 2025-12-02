using SmartStock.Models;
using SmartStock.Data;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Repository
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly DataContext _context;

        public FornecedorRepository(DataContext context)
        {
            _context = context;
        }

        public Fornecedor GetById(int id) => _context.FornecedorTable.FirstOrDefault(f => f.Id == id);

        public List<Fornecedor> GetFornecedores() => _context.FornecedorTable.ToList();

        public Fornecedor PostFornecedor(FornecedorPostDTO dto)
        {
            var fornecedor = new Fornecedor
            {
                Nome = dto.Nome,
                Cnpj = dto.CNPJ,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            _context.FornecedorTable.Add(fornecedor);
            _context.SaveChanges();
            return fornecedor;
        }

        public Fornecedor PutFornecedor(int id, FornecedorPutDTO dto)
        {
            var fornecedor = _context.FornecedorTable.FirstOrDefault(f => f.Id == id);
            if (fornecedor == null) return null;

            fornecedor.Nome = dto.Nome;
            fornecedor.Cnpj = dto.CNPJ;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;
            fornecedor.Endereco = dto.Endereco;
            fornecedor.DataAtualizacao = DateTime.Now;

            _context.SaveChanges();
            return fornecedor;
        }

        public Fornecedor PatchFornecedor(int id, FornecedorPatchDTO dto)
        {
            var fornecedor = _context.FornecedorTable.FirstOrDefault(f => f.Id == id);
            if (fornecedor == null) return null;

            if (dto.Nome != null) fornecedor.Nome = dto.Nome;
            if (dto.CNPJ != null) fornecedor.Cnpj = dto.CNPJ;
            if (dto.Telefone != null) fornecedor.Telefone = dto.Telefone;
            if (dto.Email != null) fornecedor.Email = dto.Email;
            if (dto.Endereco != null) fornecedor.Endereco = dto.Endereco;

            fornecedor.DataAtualizacao = DateTime.Now;
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
