using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Modified_CustomerAddress_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporaryDelete",
                table: "CustomerAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporaryDelete",
                table: "ContactPerson",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporaryDelete",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "IsTemporaryDelete",
                table: "ContactPerson");
        }
    }
}
