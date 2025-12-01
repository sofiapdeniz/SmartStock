namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class FornecedorPostDTO : EntidadeBase
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }

        public List<ProdutoFornecedorDTO> Produtos { get; set; }
    }
}
