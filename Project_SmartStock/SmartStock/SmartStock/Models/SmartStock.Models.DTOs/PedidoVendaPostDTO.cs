using static SmartStock.Models.PedidoCompra;
using static SmartStock.Models.PedidoVenda;

namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoVendaPostDTO
    {
        public string ClienteNome { get; set; }
        public decimal ValorTotal { get; set; }
        public TipoEntregaEnum TipoEntrega { get; set; }
        public string EnderecoEntrega { get; set; }
        public string BairroEntrega { get; set; }
        public int NumeroEnderecoEntrega { get; set; }
        public string TelefoneCliente { get; set; }
        public string? LojaRetirada { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; }
    }
}
