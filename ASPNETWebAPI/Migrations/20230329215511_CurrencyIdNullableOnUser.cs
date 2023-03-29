using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyIdNullableOnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Currencies_CurrencyId",
                table: "Users",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
