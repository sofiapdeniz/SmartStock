namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class ItemPedidoResponseDTO
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal Subtotal { get; set; }
    }
}
