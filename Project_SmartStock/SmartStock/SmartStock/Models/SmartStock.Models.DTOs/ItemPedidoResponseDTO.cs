// EM SmartStock.Models.SmartStock.Models.DTOs/ItemPedidoResponseDTO.cs

using System;

namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class ItemPedidoResponseDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
        public int Estoque { get; set; }
    }
}