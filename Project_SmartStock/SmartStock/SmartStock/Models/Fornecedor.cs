namespace SmartStock.Models
{
    public class Fornecedor : EntidadeBase
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public ICollection<FornecedorProduto> ProdutosFornecidos { get; set; } = new List<FornecedorProduto>();
    }
}
