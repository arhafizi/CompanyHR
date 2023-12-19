using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyHR.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37b5c391-fed5-43b7-9158-4ca21a77cfc6", "8dff8eb8-82c1-4851-b999-3b0fb4de92b9", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5a704eb-5d7b-432d-aedf-3508121946c0", "cd9c20ed-342e-4dc5-a22a-b75a49744ace", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37b5c391-fed5-43b7-9158-4ca21a77cfc6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5a704eb-5d7b-432d-aedf-3508121946c0");
        }
    }
}
