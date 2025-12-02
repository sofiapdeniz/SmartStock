namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoCompraPutDTO
    {
        public string NomeFornecedor { get; set; }
        public int CondicaoPagamento { get; set; }
        public string Contato { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; }
    }
}
