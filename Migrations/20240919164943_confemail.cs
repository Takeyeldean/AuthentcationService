using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthentcationServiceForTradingMarket.Migrations
{
    /// <inheritdoc />
    public partial class confemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "AspNetUsers");
        }
    }
}
