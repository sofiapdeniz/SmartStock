using System.Text.Json.Serialization;
using SmartStock.Models; // Já existe na sua amostra

namespace SmartStock.Models
{
    public class FornecedorProduto
    {
        public decimal PrecoCusto { get; set; }
        public string CodProduto { get; set; }
        public bool Ativo { get; set; }

        public int ProdutoId { get; set; }

        [JsonIgnore]
        public Produto Produto { get; set; }

        public int FornecedorId { get; set; }

        [JsonIgnore]
        public Fornecedor Fornecedor { get; set; }
    }
}