using Microsoft.EntityFrameworkCore;
using SmartStock.Models;
using System.Linq;
using Microsoft.Data.SqlClient; // Importante para capturar o erro SQL

namespace SmartStock.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options) 
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- 1. CONFIGURAÇÃO DE ITEMPEDIDO (CHAVE ESTRANGEIRA SEPARADA) ---
            
            // 1.1 Relacionamento ItemPedido -> PedidoCompra 
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.PedidoCompra)
                .WithMany(pc => pc.ItensPedido)
                .HasForeignKey(ip => ip.PedidoCompraId)
                .IsRequired(false); // Não é obrigatório
            
            // 1.2 Relacionamento ItemPedido -> PedidoVenda 
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.PedidoVenda)
                .WithMany(pv => pv.ItensPedido)
                .HasForeignKey(ip => ip.PedidoVendaId)
                .IsRequired(false); // Não é obrigatório
            
            // 1.3 Relacionamento ItemPedido -> Produto (Mantido)
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.ProdutoId);

            
            // --- 2. CONFIGURAÇÃO FORNECEDORPRODUTO (M:N) (Mantido) ---
            
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

            // 3. Configuração PedidoCompra -> Fornecedor (Mantido)
            modelBuilder.Entity<PedidoCompra>()
                .HasOne(pc => pc.Fornecedor)
                .WithMany()
                .HasForeignKey(pc => pc.FornecedorId)
                .IsRequired();
        }

        public override int SaveChanges()
        {
            // Lógica de DataCriacao/DataAtualizacao (Mantida)
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
            
            // 🚨 NOVO: BLOCO TRY-CATCH PARA CAPTURAR A EXCEÇÃO SQL 🚨
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Tenta encontrar a exceção SQL interna
                var sqlException = ex.InnerException?.InnerException as SqlException;

                if (sqlException != null)
                {
                    // Imprime no console (ou log) o erro SQL específico
                    Console.WriteLine("\n--- ERRO CRÍTICO NO SAVECHANGES (DIAGNÓSTICO SQL) ---");
                    Console.WriteLine($"Número do Erro SQL: {sqlException.Number}");
                    Console.WriteLine($"Mensagem SQL: {sqlException.Message}");
                    Console.WriteLine("------------------------------------------------------\n");
                }
                
                // Relança a exceção original para que o Service/Controller receba o erro.
                throw new InvalidOperationException("An error occurred while saving the entity changes. See the inner exception for details.", ex);
            }
        }

        public DbSet<Produto> ProdutoTable { get; set; }
        public DbSet<ItemPedido> ItemPedidoTable { get; set; }
        public DbSet<Models.PedidoVenda> PedidoVendaTable { get; set; }
        public DbSet<Models.PedidoCompra> PedidoCompraTable { get; set; }
        public DbSet<Fornecedor> FornecedorTable { get; set; }
        public DbSet<FornecedorProduto> FornecedorProdutoTable { get; set; }
    }
}