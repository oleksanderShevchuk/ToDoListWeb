using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWeb.Migrations
{
    public partial class AddNameToIdentityUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

        }
    }
}
