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
        private readonly IFornecedorRepository _fornecedor;
        private readonly IProdutoRepository _produtoRepository;

        public FornecedorService(IFornecedorRepository fornecedor, IProdutoRepository produtoRepository)
        {
            _fornecedor = fornecedor;
            _produtoRepository = produtoRepository;
        }

        public Fornecedor GetById(int id)
        {
            return _fornecedor.GetById(id);
        }

        public List<Fornecedor> GetFornecedores()
        {
            return _fornecedor.GetFornecedores();
        }

        public Fornecedor PostFornecedor(FornecedorPostDTO dto)
        {
            if (dto == null)
                throw new Exception("O corpo da requisição é inválido.");

            if (_fornecedor.GetById(dto.Id) != null)
                throw new Exception($"Já existe um fornecedor com este mesmo id={dto.Id}");

            var fornecedor = new Fornecedor
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Cnpj = dto.CNPJ,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco
            };

            foreach (var itemDto in fornecedor.ProdutosFornecidos)
            {
                var produto = _produtoRepository.GetById(itemDto.ProdutoId);
                if (produto == null)
                {
                    throw new Exception($"Produto com Id={itemDto.ProdutoId} não encontrado.");
                }

                 var ProdutosFornecidos = new FornecedorProduto
                {
                    ProdutoId = produto.Id,
                };
                fornecedor.ProdutosFornecidos.Add(ProdutosFornecidos);
            }

            return _fornecedor.PostFornecedor(dto);
        }

        public Fornecedor PutFornecedor(int id, FornecedorPutDTO dto)
        {
            var fornecedor = _fornecedor.GetById(id);
            if (fornecedor == null) return null;

            fornecedor.Nome = dto.Nome;
            fornecedor.Cnpj = dto.CNPJ;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;
            fornecedor.Endereco = dto.Endereco;

            return _fornecedor.PutFornecedor(id, dto);
        }

        public Fornecedor PatchFornecedor(int id, FornecedorPatchDTO dto)
        {
            var fornecedor = _fornecedor.GetById(id);
            if (fornecedor == null) 
                return null;

            if (dto.Nome != null) 
                fornecedor.Nome = dto.Nome;

            if (dto.CNPJ != null) 
                fornecedor.Cnpj = dto.CNPJ;

            if (dto.Telefone != null) 
                fornecedor.Telefone = dto.Telefone;

            if (dto.Email != null) 
                fornecedor.Email = dto.Email;

            if (dto.Endereco != null) 
                fornecedor.Endereco = dto.Endereco;

            return _fornecedor.PatchFornecedor(id, dto);
        }

        public Fornecedor Delete(int id)
        {
            var fornecedor = _fornecedor.GetById(id);
            if (fornecedor == null) 
                return null;

            return _fornecedor.Delete(id);
        }
    }
}
