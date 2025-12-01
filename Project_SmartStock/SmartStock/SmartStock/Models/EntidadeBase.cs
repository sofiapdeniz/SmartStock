namespace SmartStock.Models
{
    public abstract class EntidadeBase
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } 
        public DateTime DataAtualizacao { get; set; }

    }
}
