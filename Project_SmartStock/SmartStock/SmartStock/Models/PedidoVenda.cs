using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class PedidoVenda : EntidadeBase
    {
        public string ClienteNome { get; set; }
        public decimal ValorTotal { get; set; }
        public TipoEntregaEnum TipoEntrega { get; set; }
        public string EnderecoEntrega { get; set; }
        public string BairroEntrega { get; set; }
        public int NumeroEnderecoEntrega { get; set; }
        public string TelefoneCliente { get; set; }
        public string? LojaRetirada { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; }

        public enum TipoEntregaEnum
        {
            Entrega,
            Retirada
        }
    }
}
