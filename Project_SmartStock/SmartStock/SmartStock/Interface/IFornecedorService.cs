using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;

namespace SmartStock.Interface
{
    public interface IFornecedorService
    {
        List<Fornecedor> GetFornecedores();
        Fornecedor GetById(int id);
        Fornecedor PostFornecedor(FornecedorPostDTO fornecedor);
        Fornecedor PutFornecedor(int id, FornecedorPutDTO fornecedor);
        Fornecedor PatchFornecedor(int id, FornecedorPatchDTO fornecedor);
        Fornecedor Delete(int id);
    }
}
