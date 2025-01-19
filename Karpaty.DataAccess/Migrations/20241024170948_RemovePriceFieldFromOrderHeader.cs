using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karpaty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceFieldFromOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderPrice",
                table: "OrderHeaders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OrderPrice",
                table: "OrderHeaders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
