using SmartStock.Controllers;
using SmartStock.Models;
using System.Text.Json.Serialization;

namespace SmartStock.Models
{
    public class Produto : EntidadeBase
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string UnidadeMedida { get; set; }
        public int Estoque {get; set; } = 0;

        [JsonIgnore]
        public ICollection<ItemPedido> ItensPedido { get; set; }
        public ICollection<FornecedorProduto> Fornecedores { get; set; }
    }
}
