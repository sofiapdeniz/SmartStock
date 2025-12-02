using SmartStock.Controllers;
using System.Text.Json.Serialization;
using SmartStock.Models; // Certifique-se de que este using está correto

namespace SmartStock.Models
{
    public class ItemPedido
    {
        public string UnidadeMedida { get; set; }
        public decimal PrecoUnitario { get; set; }

        public int Quantidade { get; set; }

        public int PedidoId { get; set; }

        [JsonIgnore]
        public PedidoCompra Pedido { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}