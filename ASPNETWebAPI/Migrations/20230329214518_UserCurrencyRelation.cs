using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserCurrencyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrencyId",
                table: "Users",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrencyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Users");
        }
    }
}
