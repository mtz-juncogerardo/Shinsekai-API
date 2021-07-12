using Microsoft.EntityFrameworkCore.Migrations;

namespace Shinsekai_API.Migrations
{
    public partial class ShinsekaiMigration_v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesArticles_Articles_ArticleId",
                table: "PurchasesArticles");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleId",
                table: "PurchasesArticles",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesArticles_Articles_ArticleId",
                table: "PurchasesArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesArticles_Articles_ArticleId",
                table: "PurchasesArticles");

            migrationBuilder.AlterColumn<string>(
                name: "ArticleId",
                table: "PurchasesArticles",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesArticles_Articles_ArticleId",
                table: "PurchasesArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
