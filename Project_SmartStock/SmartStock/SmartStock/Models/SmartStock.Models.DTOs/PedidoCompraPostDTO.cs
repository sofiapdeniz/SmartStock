using static SmartStock.Models.PedidoCompra;

namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoCompraPostDTO : EntidadeBase
    {
        public int FornecedorId { get; set; }
        public string NomeFornecedor { get; set; }
        public int CondicaoPagamento { get; set; }
        public string Contato { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; }
    }
}
