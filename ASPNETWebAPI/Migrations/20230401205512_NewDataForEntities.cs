using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASPNETWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewDataForEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spending_SpendingType_SpendingTypeId",
                table: "Spending");

            migrationBuilder.DropForeignKey(
                name: "FK_Spending_Users_UserId",
                table: "Spending");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpendingType",
                table: "SpendingType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spending",
                table: "Spending");

            migrationBuilder.RenameTable(
                name: "SpendingType",
                newName: "SpendingTypes");

            migrationBuilder.RenameTable(
                name: "Spending",
                newName: "Spendings");

            migrationBuilder.RenameIndex(
                name: "IX_Spending_UserId",
                table: "Spendings",
                newName: "IX_Spendings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Spending_SpendingTypeId",
                table: "Spendings",
                newName: "IX_Spendings_SpendingTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpendingTypes",
                table: "SpendingTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spendings",
                table: "Spendings",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Spendings",
                columns: new[] { "Id", "Amount", "Comment", "Date", "SpendingTypeId", "UserId" },
                values: new object[,]
                {
                    { 1, 250f, "I bought a new screen", new DateTime(2023, 4, 1, 22, 55, 12, 124, DateTimeKind.Local).AddTicks(8517), 3, 1 },
                    { 2, 50.5f, "I had a nice meal at the restaurant", new DateTime(2023, 3, 31, 22, 55, 12, 124, DateTimeKind.Local).AddTicks(8559), 1, 2 },
                    { 3, 110.9f, "I went to the best hotel in town", new DateTime(2023, 3, 27, 22, 55, 12, 124, DateTimeKind.Local).AddTicks(8562), 2, 1 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Spendings_SpendingTypes_SpendingTypeId",
                table: "Spendings",
                column: "SpendingTypeId",
                principalTable: "SpendingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Spendings_Users_UserId",
                table: "Spendings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spendings_SpendingTypes_SpendingTypeId",
                table: "Spendings");

            migrationBuilder.DropForeignKey(
                name: "FK_Spendings_Users_UserId",
                table: "Spendings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpendingTypes",
                table: "SpendingTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spendings",
                table: "Spendings");

            migrationBuilder.DeleteData(
                table: "Spendings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Spendings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Spendings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "SpendingTypes",
                newName: "SpendingType");

            migrationBuilder.RenameTable(
                name: "Spendings",
                newName: "Spending");

            migrationBuilder.RenameIndex(
                name: "IX_Spendings_UserId",
                table: "Spending",
                newName: "IX_Spending_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Spendings_SpendingTypeId",
                table: "Spending",
                newName: "IX_Spending_SpendingTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpendingType",
                table: "SpendingType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spending",
                table: "Spending",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Spending_SpendingType_SpendingTypeId",
                table: "Spending",
                column: "SpendingTypeId",
                principalTable: "SpendingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Spending_Users_UserId",
                table: "Spending",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
