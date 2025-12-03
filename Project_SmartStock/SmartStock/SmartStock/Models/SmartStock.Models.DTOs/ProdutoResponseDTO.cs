namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class ProdutoResponseDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; } 
        public int Codigo { get; set; }
        public required string Descricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public required string UnidadeMedida { get; set; }
        public int Estoque { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public List<int> Fornecedores { get; set; } = new List<int>();
    }
}