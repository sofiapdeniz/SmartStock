using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class PedidoCompra : EntidadeBase
    {
        public string NomeFornecedor { get; set; }
        public int CondicaoPagamento { get; set; }
        public string Contato { get; set; }

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }

        public ICollection<ItemPedido> ItensPedido { get; set; }
    }
}
