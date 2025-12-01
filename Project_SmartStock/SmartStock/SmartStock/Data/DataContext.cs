using Microsoft.EntityFrameworkCore;
using SmartStock.Controllers;
using SmartStock.Models;

namespace SmartStock.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ItemPedido>()
                .HasKey(ip => new { ip.PedidoId, ip.ProdutoId });
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId);
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.ProdutoId);
     
            modelBuilder.Entity<FornecedorProduto>()
                .HasKey(fp => new { fp.FornecedorId, fp.ProdutoId });
            modelBuilder.Entity<FornecedorProduto>()
                .HasOne(fp => fp.Fornecedor)
                .WithMany(f => f.ProdutosFornecidos)
                .HasForeignKey(fp => fp.FornecedorId);
            modelBuilder.Entity<FornecedorProduto>()
                .HasOne(fp => fp.Produto)
                .WithMany(p => p.Fornecedores)
                .HasForeignKey(fp => fp.ProdutoId);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Fornecedor || e.Entity is Produto);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((dynamic)entry.Entity).DataCriacao = DateTime.Now;
                    ((dynamic)entry.Entity).DataAtualizacao = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    ((dynamic)entry.Entity).DataAtualizacao = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public DbSet<Produto> ProdutoTable { get; set; }
        public DbSet<ItemPedido> ItemPedidoTable { get; set; }
        public DbSet<Models.PedidoVenda> PedidoVendaTable { get; set; }
        public DbSet<Models.PedidoCompra> PedidoCompraTable { get; set; }
        public DbSet<Fornecedor> FornecedorTable { get; set; }
        public DbSet<FornecedorProduto> FornecedorProdutoTable { get; set; }
    }
}
