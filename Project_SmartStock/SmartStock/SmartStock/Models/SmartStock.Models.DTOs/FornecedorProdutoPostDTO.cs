namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class FornecedorProdutoPostDTO
    {
        public int FornecedorId { get; set; }
        public decimal PrecoCusto { get; set; }
        public string CodProduto { get; set; }
        public bool Ativo { get; set; }
    }
}