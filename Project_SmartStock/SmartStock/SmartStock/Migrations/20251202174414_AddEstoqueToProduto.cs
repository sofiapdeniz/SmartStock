using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Migrations
{
    /// <inheritdoc />
    public partial class AddEstoqueToProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadTimeDias",
                table: "FornecedorProdutoTable");

            migrationBuilder.DropColumn(
                name: "UltimaAtualizacaoPreco",
                table: "FornecedorProdutoTable");

            migrationBuilder.AddColumn<int>(
                name: "Estoque",
                table: "ProdutoTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FornecedorId",
                table: "PedidoCompraTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoCompraTable_FornecedorId",
                table: "PedidoCompraTable",
                column: "FornecedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoCompraTable_FornecedorTable_FornecedorId",
                table: "PedidoCompraTable",
                column: "FornecedorId",
                principalTable: "FornecedorTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoCompraTable_FornecedorTable_FornecedorId",
                table: "PedidoCompraTable");

            migrationBuilder.DropIndex(
                name: "IX_PedidoCompraTable_FornecedorId",
                table: "PedidoCompraTable");

            migrationBuilder.DropColumn(
                name: "Estoque",
                table: "ProdutoTable");

            migrationBuilder.DropColumn(
                name: "FornecedorId",
                table: "PedidoCompraTable");

            migrationBuilder.AddColumn<int>(
                name: "LeadTimeDias",
                table: "FornecedorProdutoTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaAtualizacaoPreco",
                table: "FornecedorProdutoTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
