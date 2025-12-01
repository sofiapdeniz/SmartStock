namespace SmartStock.Models
{
    public class FornecedorProduto : EntidadeBase
    {
        public decimal PrecoCusto { get; set; }
        public string CodProduto { get; set; }
        public bool Ativo { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}
