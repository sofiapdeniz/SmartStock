using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStock.Migrations
{
    /// <inheritdoc />
    public partial class FixPedidoCompraRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoCompraId",
                table: "ItemPedidoTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoId",
                table: "ItemPedidoTable");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "FornecedorProdutoTable");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "FornecedorProdutoTable");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FornecedorProdutoTable");

            migrationBuilder.RenameColumn(
                name: "PedidoCompraId",
                table: "ItemPedidoTable",
                newName: "PedidoVendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPedidoTable_PedidoCompraId",
                table: "ItemPedidoTable",
                newName: "IX_ItemPedidoTable_PedidoVendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoId",
                table: "ItemPedidoTable",
                column: "PedidoId",
                principalTable: "PedidoCompraTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoId",
                table: "ItemPedidoTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoVendaId",
                table: "ItemPedidoTable");

            migrationBuilder.RenameColumn(
                name: "PedidoVendaId",
                table: "ItemPedidoTable",
                newName: "PedidoCompraId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPedidoTable_PedidoVendaId",
                table: "ItemPedidoTable",
                newName: "IX_ItemPedidoTable_PedidoCompraId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "FornecedorProdutoTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "FornecedorProdutoTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FornecedorProdutoTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoCompraTable_PedidoCompraId",
                table: "ItemPedidoTable",
                column: "PedidoCompraId",
                principalTable: "PedidoCompraTable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedidoTable_PedidoVendaTable_PedidoId",
                table: "ItemPedidoTable",
                column: "PedidoId",
                principalTable: "PedidoVendaTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
