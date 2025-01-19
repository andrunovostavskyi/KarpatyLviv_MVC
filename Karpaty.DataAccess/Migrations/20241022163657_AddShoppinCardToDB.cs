using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Karpaty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppinCardToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount10",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discount15",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ShoppingCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    AplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCards_AspNetUsers_AplicationUserId",
                        column: x => x.AplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCards_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_AplicationUserId",
                table: "ShoppingCards",
                column: "AplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCards_ProductId",
                table: "ShoppingCards",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCards");

            migrationBuilder.AddColumn<int>(
                name: "Discount10",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Discount15",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Discount10", "Discount15" },
                values: new object[] { 0, 0 });
        }
    }
}
