using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArchivingAndBacklogFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Columns_ColumnId",
                table: "WorkItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColumnId",
                table: "WorkItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "WorkItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "WorkItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Columns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Boards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_BoardId",
                table: "WorkItems",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Boards_BoardId",
                table: "WorkItems",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Columns_ColumnId",
                table: "WorkItems",
                column: "ColumnId",
                principalTable: "Columns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Boards_BoardId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Columns_ColumnId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_BoardId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Boards");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColumnId",
                table: "WorkItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Columns_ColumnId",
                table: "WorkItems",
                column: "ColumnId",
                principalTable: "Columns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
