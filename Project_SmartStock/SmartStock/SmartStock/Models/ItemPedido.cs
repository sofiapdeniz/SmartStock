// EM ItemPedido.cs

using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class ItemPedido
    {
        public int Id { get; set; } // Chave primária simples
        public string UnidadeMedida { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }

        // --- Novas Chaves Estrangeiras (FKs) e Propriedades de Navegação ---

        // 1. Vínculo com PedidoCompra
        public int? PedidoCompraId { get; set; } // Opcional (Nullable int)
        [JsonIgnore]
        public PedidoCompra PedidoCompra { get; set; }
        public int? PedidoVendaId { get; set; }
        [JsonIgnore]
        public PedidoVenda PedidoVenda { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}