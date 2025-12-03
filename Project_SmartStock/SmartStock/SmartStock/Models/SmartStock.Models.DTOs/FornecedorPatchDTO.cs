namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class FornecedorPatchDTO
    {
        public required string Nome { get; set; }
        public required string Cnpj { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public required string Endereco { get; set; }
    }
}
