// EM SmartStock.Services/FornecedorService.cs

using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System;
using System.Collections.Generic;

namespace SmartStock.Services
{
    public class FornecedorService : IFornecedorService
    {
        // CORREÇÃO: Tipo alterado para a interface ajustada
        private readonly IFornecedorRepository _fornecedorRepository; 
        private readonly IProdutoRepository _produtoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository, IProdutoRepository produtoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _produtoRepository = produtoRepository;
        }

        public Fornecedor GetById(int id)
        {
            return _fornecedorRepository.GetById(id);
        }

        public List<Fornecedor> GetFornecedores()
        {
            return _fornecedorRepository.GetFornecedores();
        }

        public Fornecedor PostFornecedor(FornecedorPostDTO dto)
        {
            if (dto == null)
                throw new Exception("O corpo da requisição é inválido.");

            // **CORREÇÃO:** Removemos a verificação de dto.Id e a tentativa de relacionar produtos.
            // O ID é gerado automaticamente, e a relação é feita pelo Produto.
            
            var fornecedor = new Fornecedor
            {
                Nome = dto.Nome,
                Cnpj = dto.Cnpj,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco
                // DataCriacao/DataAtualizacao serão preenchidas no DataContext
            };

            // Chamada ao repositório com a Entidade.
            return _fornecedorRepository.PostFornecedor(fornecedor); 
        }

        public Fornecedor PutFornecedor(int id, FornecedorPutDTO dto)
        {
            var fornecedor = _fornecedorRepository.GetById(id);
            if (fornecedor == null) return null;

            // Mapeamento das propriedades do DTO para a Entidade
            fornecedor.Nome = dto.Nome;
            fornecedor.Cnpj = dto.Cnpj;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;
            fornecedor.Endereco = dto.Endereco;
            // DataAtualizacao será preenchida no DataContext

            // Chamada ao repositório com a Entidade.
            return _fornecedorRepository.PutFornecedor(fornecedor);
        }

        public Fornecedor PatchFornecedor(int id, FornecedorPatchDTO dto)
        {
            var fornecedor = _fornecedorRepository.GetById(id);
            if (fornecedor == null) 
                return null;

            // Aplicação dos valores do DTO
            if (dto.Nome != null) 
                fornecedor.Nome = dto.Nome;

            if (dto.Cnpj != null) 
                fornecedor.Cnpj = dto.Cnpj;

            if (dto.Telefone != null) 
                fornecedor.Telefone = dto.Telefone;

            if (dto.Email != null) 
                fornecedor.Email = dto.Email;

            if (dto.Endereco != null) 
                fornecedor.Endereco = dto.Endereco;
            // DataAtualizacao será preenchida no DataContext

            // Chamada ao repositório com a Entidade.
            return _fornecedorRepository.PatchFornecedor(fornecedor);
        }

        public Fornecedor Delete(int id)
        {
            var fornecedor = _fornecedorRepository.GetById(id);
            if (fornecedor == null) 
                return null;

            return _fornecedorRepository.Delete(id);
        }
    }
}