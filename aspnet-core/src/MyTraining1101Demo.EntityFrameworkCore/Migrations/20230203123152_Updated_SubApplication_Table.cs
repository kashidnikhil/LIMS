using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Updated_SubApplication_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubApplications_Applications_ApplicationsId",
                table: "SubApplications");

            migrationBuilder.DropIndex(
                name: "IX_SubApplications_ApplicationsId",
                table: "SubApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationsId",
                table: "SubApplications");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubApplications_ApplicationId",
                table: "SubApplications",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubApplications_Applications_ApplicationId",
                table: "SubApplications",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubApplications_Applications_ApplicationId",
                table: "SubApplications");

            migrationBuilder.DropIndex(
                name: "IX_SubApplications_ApplicationId",
                table: "SubApplications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationsId",
                table: "SubApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubApplications_ApplicationsId",
                table: "SubApplications",
                column: "ApplicationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubApplications_Applications_ApplicationsId",
                table: "SubApplications",
                column: "ApplicationsId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
