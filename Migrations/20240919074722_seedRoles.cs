using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthentcationServiceForTradingMarket.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "AspNetRoles",
                 columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                 values: new object[] { Guid.NewGuid().ToString(), "User", "USER".ToUpper(), Guid.NewGuid().ToString() }
             );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
