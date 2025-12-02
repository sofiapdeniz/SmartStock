using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SmartStock.Data;
using System.IO;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        // Constrói a configuração para localizar e ler o appsettings.json
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Obtém a Connection String que está configurada no appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A ConnectionString 'DefaultConnection' não foi encontrada nas configurações.");
        }

        // Configura o DbContext com a string de conexão obtida
        var builder = new DbContextOptionsBuilder<DataContext>();
        // Lembre-se: use UseSqlServer() se estiver usando SQL Server
        builder.UseSqlServer(connectionString);

        return new DataContext(builder.Options);
    }
}