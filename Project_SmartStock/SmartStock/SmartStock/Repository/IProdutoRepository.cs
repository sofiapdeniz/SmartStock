using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Repository
{
    public interface IProdutoRepository
    {
        List<Produto> GetProdutos();
        Produto GetById(int id);
        Produto PostProduto(ProdutoPostDTO produto);
        Produto PutProduto(int id, ProdutoPutDTO produto);
        Produto PatchProduto(int id, ProdutoPatchDTO produto);
        Produto Delete(int id);

        Produto Update(Produto produto);
    }
}
