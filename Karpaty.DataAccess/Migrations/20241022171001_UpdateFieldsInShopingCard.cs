using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karpaty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsInShopingCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ShoppingCards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "ShoppingCards");
        }
    }
}
