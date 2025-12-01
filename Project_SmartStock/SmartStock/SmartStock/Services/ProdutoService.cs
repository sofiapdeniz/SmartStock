using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System.Data;

namespace SmartStock.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutoService(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
        }
        public Produto Delete(int id)
        {
            var produto = _produtoRepository.GetById(id);
            if (produto == null)
                return null;

            return _produtoRepository.Delete(id);
        }

        public Produto GetById(int id)
        {
            return _produtoRepository.GetById(id);
        }

        public List<Produto> GetProdutos()
        {
            return _produtoRepository.GetProdutos();
        }

        public Produto PatchProduto(int id, ProdutoPatchDTO dto)
        {
            var produto = _produtoRepository.GetById(id);

            if (produto == null)
                return null;

            if (dto.Nome != null)
                produto.Nome = dto.Nome;

            if (dto.Codigo != null)
                produto.Codigo = dto.Codigo.Value;

            if (dto.Descricao != null)
               produto.Descricao = dto.Descricao;

            if (dto.PrecoUnitario != null)
                produto.PrecoUnitario = dto.PrecoUnitario.Value;

            if (dto.UnidadeMedida != null)
                produto.UnidadeMedida = dto.UnidadeMedida;

            return _produtoRepository.PatchProduto(id, dto);


        }

        
        public Produto PostProduto(ProdutoPostDTO dto)
        {
            if (dto == null)
                throw new Exception("O corpo da requisição é inválido.");

            if (_produtoRepository.GetById(dto.Id) != null)
                throw new Exception($"Já existe um produto com este mesmo código={dto.Codigo}");
        
            var produto = new Produto
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                PrecoUnitario = dto.PrecoUnitario,
                UnidadeMedida = dto.UnidadeMedida
            };

           return _produtoRepository.PostProduto(dto);
        }

        public Produto PutProduto(int id, ProdutoPutDTO dto)
        {
            var produto = _produtoRepository.GetById(id);

            if (produto == null)
                return null;

            produto.Nome = dto.Nome;
            produto.Codigo = dto.Codigo;
            produto.Descricao = dto.Descricao;
            produto.PrecoUnitario = dto.PrecoUnitario;
            produto.UnidadeMedida = dto.UnidadeMedida;

            return _produtoRepository.PutProduto(id, dto);
        }
    }
}
