namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoCompraResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public int FornecedorId { get; set; }
        public string NomeFornecedor { get; set; }
        public string Contato { get; set; }
        
        public int CondicaoPagamento { get; set; }
        
        public List<ItemPedidoResponseDTO> Itens { get; set; }
    }
}