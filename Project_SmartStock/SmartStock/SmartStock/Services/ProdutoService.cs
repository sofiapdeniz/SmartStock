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

        // Note: IFornecedorRepository é necessário para verificar se o fornecedor existe.
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
            // Nota: Para carregar fornecedores, você precisará de .Include() no Repository
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
                throw new ArgumentException("O corpo da requisição é inválido.");

            // É mais comum verificar se o Código (Unique) já existe, não o Id, que deve ser 0 em um POST.
            // Para simplificar, vou remover a verificação de dto.Id == 0, mas se Código for unique, verifique-o.
            // if (_produtoRepository.GetByCodigo(dto.Codigo) != null) 
            //    throw new Exception($"Já existe um produto com este mesmo código={dto.Codigo}");
        
            // 1. Cria a Entidade Produto, inicializando a coleção Fornecedores
            var produto = new Produto
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao,
                PrecoUnitario = dto.PrecoUnitario,
                UnidadeMedida = dto.UnidadeMedida,
                Estoque = 0,
                Fornecedores = new List<FornecedorProduto>() // Inicializa a coleção M:N
            };

            // 2. Processa as relações FornecedorProduto
            if (dto.Fornecedores != null && dto.Fornecedores.Any())
            {
                foreach (var fpDto in dto.Fornecedores)
                {
                    // VERIFICAÇÃO DE CHAVE ESTRANGEIRA (Garante que o FornecedorId existe)
                    if (_fornecedorRepository.GetById(fpDto.FornecedorId) == null)
                    {
                        throw new KeyNotFoundException($"Fornecedor com ID={fpDto.FornecedorId} não encontrado.");
                    }

                    // CRIAÇÃO DA ENTIDADE M:N
                    produto.Fornecedores.Add(new FornecedorProduto
                    {
                        FornecedorId = fpDto.FornecedorId,
                        PrecoCusto = fpDto.PrecoCusto,
                        CodProduto = fpDto.CodProduto,
                        Ativo = fpDto.Ativo,
                        // Não precisamos definir ProdutoId, o EF fará isso quando salvarmos o Produto.
                    });
                }
            }

            // 3. O Repository agora recebe a Entidade Produto COMPLETA para salvar.
            return _produtoRepository.PostProduto(produto);
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