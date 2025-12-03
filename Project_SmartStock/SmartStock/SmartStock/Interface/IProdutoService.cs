using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using System.Collections.Generic;

namespace SmartStock.Interface
{
    public interface IProdutoService 
    {
        List<ProdutoResponseDTO> GetProdutos(); 
        ProdutoResponseDTO? GetById(int id);
        Produto? PostProduto(ProdutoPostDTO produto);
        Produto? PutProduto(int id, ProdutoPutDTO produto);
        Produto? PatchProduto(int id, ProdutoPatchDTO produto);
        Produto? Delete(int id);
    }
}