using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Migrations
{
    /// <inheritdoc />
    public partial class SepararTudoItemPedidoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. DROP FK Antiga (Chave antiga)
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoId",
                table: "ItemPedidoTable");

            // 2. DROP PK Antiga (Chave composta)
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPedidoTable",
                table: "ItemPedidoTable");

            // 3. RENOMEIA a antiga FK PedidoId para o novo PK Id
            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "ItemPedidoTable",
                newName: "Id");

            // 4. 🚨 BLOCO SQL PARA RECRIAR A COLUNA 'Id' COMO IDENTITY E ADICIONAR AS FKs
            // Esta é a solução para o erro persistente de IDENTITY no SQL Server.
            migrationBuilder.Sql(
                @"
                -- 4.1 Cria uma tabela temporária para segurar os dados existentes
                CREATE TABLE #ItemPedidoTable_Temp (
                    Id INT NOT NULL, 
                    ProdutoId INT NOT NULL, 
                    UnidadeMedida NVARCHAR(MAX) NOT NULL, 
                    PrecoUnitario DECIMAL(18,2) NOT NULL, 
                    Quantidade INT NOT NULL
                );
                
                -- 4.2 Copia SOMENTE as colunas que EXISTEM (Id/PedidoId antigo e os demais) para a temporária
                INSERT INTO #ItemPedidoTable_Temp (Id, ProdutoId, UnidadeMedida, PrecoUnitario, Quantidade) 
                SELECT Id, ProdutoId, UnidadeMedida, PrecoUnitario, Quantidade FROM ItemPedidoTable;
                
                -- 4.3 Remove a tabela original
                DROP TABLE ItemPedidoTable;

                -- 4.4 Recria a tabela original, agora com Id IDENTITY e as novas colunas FKs (PedidoCompraId e PedidoVendaId)
                CREATE TABLE ItemPedidoTable (
                    Id INT IDENTITY(1,1) NOT NULL, 
                    ProdutoId INT NOT NULL, 
                    UnidadeMedida NVARCHAR(MAX) NOT NULL, 
                    PrecoUnitario DECIMAL(18,2) NOT NULL, 
                    Quantidade INT NOT NULL,
                    PedidoCompraId INT NULL,
                    PedidoVendaId INT NULL
                );
                
                -- 4.5 Copia os dados de volta, forçando a preservação do Id original (SET IDENTITY_INSERT ON/OFF)
                SET IDENTITY_INSERT ItemPedidoTable ON;
                INSERT INTO ItemPedidoTable (Id, ProdutoId, UnidadeMedida, PrecoUnitario, Quantidade) 
                SELECT Id, ProdutoId, UnidadeMedida, PrecoUnitario, Quantidade FROM #ItemPedidoTable_Temp;
                SET IDENTITY_INSERT ItemPedidoTable OFF;
                
                -- 4.6 Remove a tabela temporária
                DROP TABLE #ItemPedidoTable_Temp;"
            );

            // 5. ADICIONA NOVA PK (Chave primária simples)
            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPedidoTable",
                table: "ItemPedidoTable",
                column: "Id");

            // 6. CRIA ÍNDICES E NOVAS FKs (EF Core continua responsável por estas relações)
            
            // Index e FK para PedidoCompra
            migrationBuilder.CreateIndex(
                name: "IX_ItemPedidoTable_PedidoCompraId",
                table: "ItemPedidoTable",
                column: "PedidoCompraId");
                
            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoCompraId",
                table: "ItemPedidoTable",
                column: "PedidoCompraId",
                principalTable: "PedidoCompraTable",
                principalColumn: "Id");
                
            // Index e FK para PedidoVenda
            migrationBuilder.CreateIndex(
                name: "IX_ItemPedidoTable_PedidoVendaId",
                table: "ItemPedidoTable",
                column: "PedidoVendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoVendaId",
                table: "ItemPedidoTable",
                column: "PedidoVendaId",
                principalTable: "PedidoVendaTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // O código Down faz o caminho inverso, para caso você precise reverter a migração.
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoCompraId",
                table: "ItemPedidoTable");
            
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoVendaId",
                table: "ItemPedidoTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPedidoTable",
                table: "ItemPedidoTable");

            migrationBuilder.DropIndex(
                name: "IX_ItemPedidoTable_PedidoCompraId",
                table: "ItemPedidoTable");
            
            migrationBuilder.DropIndex(
                name: "IX_ItemPedidoTable_PedidoVendaId",
                table: "ItemPedidoTable");
            
            // Aqui, a reversão exigiria SQL Bruto para remover o IDENTITY, mas
            // confiamos no código gerado pelo EF Core para simplificar o Down.
            // Contudo, as colunas PedidoCompraId e PedidoVendaId precisam ser removidas
            // antes de recriar a chave composta.
            migrationBuilder.DropColumn(
                name: "PedidoVendaId",
                table: "ItemPedidoTable");

            migrationBuilder.DropColumn(
                name: "PedidoCompraId",
                table: "ItemPedidoTable");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemPedidoTable",
                newName: "PedidoId");
            
            migrationBuilder.AlterColumn<int>(
                name: "PedidoId",
                table: "ItemPedidoTable",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPedidoTable",
                table: "ItemPedidoTable",
                columns: new[] { "PedidoId", "ProdutoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoId",
                table: "ItemPedidoTable",
                column: "PedidoId",
                principalTable: "PedidoCompraTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}