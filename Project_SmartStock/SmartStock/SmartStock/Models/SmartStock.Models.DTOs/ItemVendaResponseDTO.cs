namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class ItemVendaResponseDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } 
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
        public int EstoqueAtual { get; set; }
    }
}