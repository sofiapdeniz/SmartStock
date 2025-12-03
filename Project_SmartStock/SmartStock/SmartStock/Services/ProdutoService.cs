// EM SmartStock.Services/ProdutoService.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Interface;
using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using SmartStock.Repository;
using System.Data;
using System.Linq; // NECESSÁRIO para usar Select e ToList
using System.Collections.Generic; // NECESSÁRIO para usar List<T>
using System; // NECESSÁRIO para usar ArgumentException e DateTime

namespace SmartStock.Services
{
    // AVISO: IProdutoService DEVE ter os métodos GET retornando ProdutoResponseDTO
    public class ProdutoService : IProdutoService 
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutoService(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
        }
        
        // CORREÇÃO CS8603: Adiciona '?'
        public Produto? Delete(int id)
        {
            var produto = _produtoRepository.GetById(id);
            if (produto == null)
                return null; // OK

            return _produtoRepository.Delete(id);
        }

        // CORREÇÃO CS0738: Retorna ProdutoResponseDTO? (para coincidir com a interface)
        // LÓGICA DE MAPEAMENTO RESTAURADA
        public ProdutoResponseDTO? GetById(int id)
        {
            var produto = _produtoRepository.GetById(id);
            if (produto == null) return null; // OK
            return MapToResponseDTO(produto);
        }

        // CORREÇÃO CS0738: Retorna List<ProdutoResponseDTO> (para coincidir com a interface)
        // LÓGICA DE MAPEAMENTO RESTAURADA
        public List<ProdutoResponseDTO> GetProdutos()
        {
            var produtos = _produtoRepository.GetProdutos();
            // Mapeia cada entidade Produto para seu DTO correspondente
            return produtos.Select(MapToResponseDTO).ToList();
        }

        // CORREÇÃO CS8603: Adiciona '?'
        public Produto? PatchProduto(int id, ProdutoPatchDTO dto)
        {
            var produto = _produtoRepository.GetById(id);

            if (produto == null)
                return null; // OK

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

            // OBS: Verifique se o PatchProduto no Repository aceita (id, dto) ou (entidade)
            return _produtoRepository.PatchProduto(id, dto); 
        }

        
        // Mantém Produto, se o repositório garantir que nunca falhará ou lançará exceção.
        public Produto PostProduto(ProdutoPostDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("O corpo da requisição é inválido.");
        
            var produto = new Produto
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                PrecoUnitario = dto.PrecoUnitario,
                UnidadeMedida = dto.UnidadeMedida,
                Estoque = 0,
                Fornecedores = new List<FornecedorProduto>()
            };

            if (dto.Fornecedores != null && dto.Fornecedores.Any())
            {
                foreach (var fpDto in dto.Fornecedores)
                {
                    if (_fornecedorRepository.GetById(fpDto.FornecedorId) == null)
                    {
                        throw new KeyNotFoundException($"Fornecedor com ID={fpDto.FornecedorId} não encontrado.");
                    }

                    produto.Fornecedores.Add(new FornecedorProduto
                    {
                        FornecedorId = fpDto.FornecedorId,
                        PrecoCusto = fpDto.PrecoCusto,
                        CodProduto = fpDto.CodProduto,
                        Ativo = fpDto.Ativo,
                    });
                }
            }

            return _produtoRepository.PostProduto(produto);
        }

        // CORREÇÃO CS8603: Adiciona '?'
        public Produto? PutProduto(int id, ProdutoPutDTO dto)
        {
            var produto = _produtoRepository.GetById(id);

            if (produto == null)
                return null; // OK

            produto.Nome = dto.Nome;
            produto.Codigo = dto.Codigo;
            produto.Descricao = dto.Descricao;
            produto.PrecoUnitario = dto.PrecoUnitario;
            produto.UnidadeMedida = dto.UnidadeMedida;

            return _produtoRepository.PutProduto(id, dto);
        }

        // CORREÇÃO CS8603: Adiciona '?' (Permite que o mapeamento retorne null se o produto for null)
        private ProdutoResponseDTO? MapToResponseDTO(Produto? produto)
        {
            if (produto == null) return null; // OK

            return new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                PrecoUnitario = produto.PrecoUnitario,
                UnidadeMedida = produto.UnidadeMedida,
                Estoque = produto.Estoque,
                DataCriacao = produto.DataCriacao,
                DataAtualizacao = produto.DataAtualizacao,
                
                Fornecedores = produto.Fornecedores?
                    .Select(fp => fp.FornecedorId)
                    .ToList()
                    ?? new List<int>()
            };
        }
    }
}