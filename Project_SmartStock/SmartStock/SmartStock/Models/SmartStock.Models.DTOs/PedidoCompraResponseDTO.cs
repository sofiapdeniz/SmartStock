namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoCompraResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        // Informações resumidas do Fornecedor e Contato
        public int FornecedorId { get; set; }
        public string NomeFornecedor { get; set; }
        public string Contato { get; set; }
        
        // Detalhes da Compra
        public int CondicaoPagamento { get; set; }
        
        // Itens
        public List<ItemPedidoResponseDTO> Itens { get; set; }
    }
}