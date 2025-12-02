using Microsoft.EntityFrameworkCore;
using SmartStock.Controllers;
using SmartStock.Models;
using System.Linq;

namespace SmartStock.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options) 
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- 1. CONFIGURAÇÃO DE ITEMPEDIDO (CHAVE COMPOSTA) ---
            
            // 1.1 Chave Composta: PedidoId + ProdutoId
            modelBuilder.Entity<ItemPedido>()
                .HasKey(ip => new { ip.PedidoId, ip.ProdutoId });
            
            // 1.2 Relacionamento ItemPedido -> PedidoCompra 
            modelBuilder.Entity<ItemPedido>()
                .HasOne<PedidoCompra>(ip => ip.Pedido as PedidoCompra)
                .WithMany(pc => pc.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId)
                .IsRequired(); 
            
            // 1.3 Relacionamento ItemPedido -> Produto 
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.ProdutoId);

            
            // --- 2. CONFIGURAÇÃO FORNECEDORPRODUTO (M:N) ---
            
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

            // 3. Configuração PedidoCompra -> Fornecedor 
            modelBuilder.Entity<PedidoCompra>()
                .HasOne(pc => pc.Fornecedor)
                .WithMany()
                .HasForeignKey(pc => pc.FornecedorId)
                .IsRequired();
        }

        public override int SaveChanges()
        {
            // --- CORREÇÃO: ITEMPEDIDO FOI REMOVIDO DO FILTRO ---
            // Agora apenas entidades que herdam de EntidadeBase (e têm DataCriacao/DataAtualizacao) são processadas.
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Fornecedor 
                         || e.Entity is Produto 
                         || e.Entity is Models.PedidoCompra 
                         || e.Entity is Models.PedidoVenda); 

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