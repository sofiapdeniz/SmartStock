using System.Text.Json.Serialization;
using SmartStock.Models;

namespace SmartStock.Models
{
    public class FornecedorProduto
    {
        public decimal PrecoCusto { get; set; }
        public string CodProduto { get; set; }
        public bool Ativo { get; set; }

        public int ProdutoId { get; set; }

        [JsonIgnore]
        public Produto Produto { get; set; } // O FornecedorProdutoPostDTO não precisa disso.

        public int FornecedorId { get; set; }

        [JsonIgnore]
        public Fornecedor Fornecedor { get; set; } // O FornecedorProdutoPostDTO não precisa disso.
    }
}