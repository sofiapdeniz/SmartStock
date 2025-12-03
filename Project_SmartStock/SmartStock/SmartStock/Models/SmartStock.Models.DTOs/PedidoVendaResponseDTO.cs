namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class PedidoVendaResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public decimal ValorTotal { get; set; }
        
        // Detalhes do Cliente
        public string ClienteNome { get; set; }
        public string TelefoneCliente { get; set; }

        // Detalhes de Entrega/Retirada
        public int TipoEntrega { get; set; } // Usando int para o Enum
        public string? EnderecoEntrega { get; set; }
        public string? BairroEntrega { get; set; }
        public int? NumeroEnderecoEntrega { get; set; }
        public string? LojaRetirada { get; set; } 
        
        public List<ItemVendaResponseDTO> Itens { get; set; } = new List<ItemVendaResponseDTO>();
    }
}