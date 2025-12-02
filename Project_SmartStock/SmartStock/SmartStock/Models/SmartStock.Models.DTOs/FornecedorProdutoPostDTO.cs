namespace SmartStock.Models.SmartStock.Models.DTOs
{
    // Novo DTO de relação para receber dados da tabela M:N
    public class FornecedorProdutoPostDTO
    {
        // Precisamos apenas dos IDs e dos dados da relação (PrecoCusto, etc.)
        public int FornecedorId { get; set; }
        public decimal PrecoCusto { get; set; }
        public string CodProduto { get; set; }
        public bool Ativo { get; set; }
    }
}