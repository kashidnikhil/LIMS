using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Updated_Test_Master_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubApplicationId",
                table: "Test",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Test_SubApplicationId",
                table: "Test",
                column: "SubApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_SubApplications_SubApplicationId",
                table: "Test",
                column: "SubApplicationId",
                principalTable: "SubApplications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_SubApplications_SubApplicationId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_SubApplicationId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "SubApplicationId",
                table: "Test");
        }
    }
}
