using SmartStock.Models;
using SmartStock.Models.SmartStock.Models.DTOs;
using System.Collections.Generic;

namespace SmartStock.Repository
{
    public interface IFornecedorRepository
    {
        List<Fornecedor> GetFornecedores();
        Fornecedor GetById(int id);
        Fornecedor PostFornecedor(Fornecedor fornecedor);
        Fornecedor PutFornecedor(Fornecedor fornecedor);
        Fornecedor PatchFornecedor(Fornecedor fornecedor);
        Fornecedor Delete(int id);
    }
}