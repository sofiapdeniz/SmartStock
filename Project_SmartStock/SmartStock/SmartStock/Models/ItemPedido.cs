using SmartStock.Controllers;
using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class ItemPedido : EntidadeBase
    {
        public string UnidadeMedida { get; set; }
        public decimal PrecoUnitario { get; set; }

        public int Quantidade { get; set; }

        public int PedidoId { get; set; }
        public PedidoVenda Pedido { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}
