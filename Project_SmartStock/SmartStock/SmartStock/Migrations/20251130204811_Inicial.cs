using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FornecedorTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoCompraTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFornecedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CondicaoPagamento = table.Column<int>(type: "int", nullable: false),
                    Contato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoCompraTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoVendaTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoEntrega = table.Column<int>(type: "int", nullable: false),
                    EnderecoEntrega = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BairroEntrega = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroEnderecoEntrega = table.Column<int>(type: "int", nullable: false),
                    TelefoneCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LojaRetirada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoVendaTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadeMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FornecedorProdutoTable",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    PrecoCusto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeadTimeDias = table.Column<int>(type: "int", nullable: false),
                    UltimaAtualizacaoPreco = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorProdutoTable", x => new { x.FornecedorId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_FornecedorProdutoTable_FornecedorTable_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "FornecedorTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FornecedorProdutoTable_ProdutoTable_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProdutoTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemPedidoTable",
                columns: table => new
                {
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    UnidadeMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PedidoCompraId = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPedidoTable", x => new { x.PedidoId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoCompraId",
                        column: x => x.PedidoCompraId,
                        principalTable: "PedidoCompraTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "PedidoVendaTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPedidoTable_ProdutoTable_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProdutoTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FornecedorProdutoTable_ProdutoId",
                table: "FornecedorProdutoTable",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedidoTable_PedidoCompraId",
                table: "ItemPedidoTable",
                column: "PedidoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedidoTable_ProdutoId",
                table: "ItemPedidoTable",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FornecedorProdutoTable");

            migrationBuilder.DropTable(
                name: "ItemPedidoTable");

            migrationBuilder.DropTable(
                name: "FornecedorTable");

            migrationBuilder.DropTable(
                name: "PedidoCompraTable");

            migrationBuilder.DropTable(
                name: "PedidoVendaTable");

            migrationBuilder.DropTable(
                name: "ProdutoTable");
        }
    }
}
