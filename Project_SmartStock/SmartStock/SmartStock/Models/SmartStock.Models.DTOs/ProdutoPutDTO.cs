namespace SmartStock.Models.SmartStock.Models.DTOs
{
    public class ProdutoPutDTO : EntidadeBase
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public string UnidadeMedida { get; set; }
        public List<FornecedorProdutoPostDTO>? Fornecedores { get; set; }

    }
}
