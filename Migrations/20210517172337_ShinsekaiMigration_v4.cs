using Microsoft.EntityFrameworkCore.Migrations;

namespace Shinsekai_API.Migrations
{
    public partial class ShinsekaiMigration_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesArticles_Purchases_PurchaseId",
                table: "PurchasesArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuthParams_AuthParamsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuthParamsId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "AuthParamsId",
                table: "Users",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseId",
                table: "PurchasesArticles",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PurchasesArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Purchases",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<int>(
                name: "CashPoints",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthParamsId",
                table: "Users",
                column: "AuthParamsId",
                unique: true,
                filter: "[AuthParamsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesArticles_Purchases_PurchaseId",
                table: "PurchasesArticles",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuthParams_AuthParamsId",
                table: "Users",
                column: "AuthParamsId",
                principalTable: "AuthParams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesArticles_Purchases_PurchaseId",
                table: "PurchasesArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuthParams_AuthParamsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuthParamsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PurchasesArticles");

            migrationBuilder.DropColumn(
                name: "CashPoints",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Purchases");

            migrationBuilder.AlterColumn<string>(
                name: "AuthParamsId",
                table: "Users",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseId",
                table: "PurchasesArticles",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Purchases",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthParamsId",
                table: "Users",
                column: "AuthParamsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_UserId",
                table: "Purchases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesArticles_Purchases_PurchaseId",
                table: "PurchasesArticles",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuthParams_AuthParamsId",
                table: "Users",
                column: "AuthParamsId",
                principalTable: "AuthParams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
