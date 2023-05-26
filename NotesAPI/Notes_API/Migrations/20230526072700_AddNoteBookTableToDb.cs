using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes_API.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteBookTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoteBookId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NoteBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteBooks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NoteBookId",
                table: "Notes",
                column: "NoteBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_NoteBooks_NoteBookId",
                table: "Notes",
                column: "NoteBookId",
                principalTable: "NoteBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_NoteBooks_NoteBookId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "NoteBooks");

            migrationBuilder.DropIndex(
                name: "IX_Notes_NoteBookId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NoteBookId",
                table: "Notes");
        }
    }
}
