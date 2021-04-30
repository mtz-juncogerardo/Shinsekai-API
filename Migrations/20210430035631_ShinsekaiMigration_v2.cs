using Microsoft.EntityFrameworkCore.Migrations;

namespace Shinsekai_API.Migrations
{
    public partial class ShinsekaiMigration_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_OriginalsReplicas_OriginalReplicaId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "OriginalsReplicas");

            migrationBuilder.DropIndex(
                name: "IX_Articles_OriginalReplicaId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "OriginalReplicaId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "OriginalSerial",
                table: "Articles",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Originals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ArticleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Originals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Originals_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Originals_ArticleId",
                table: "Originals",
                column: "ArticleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Originals");

            migrationBuilder.DropColumn(
                name: "OriginalSerial",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "OriginalReplicaId",
                table: "Articles",
                type: "nvarchar(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OriginalsReplicas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    OriginalArticleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ReplicaArticleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalsReplicas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_OriginalReplicaId",
                table: "Articles",
                column: "OriginalReplicaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_OriginalsReplicas_OriginalReplicaId",
                table: "Articles",
                column: "OriginalReplicaId",
                principalTable: "OriginalsReplicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
