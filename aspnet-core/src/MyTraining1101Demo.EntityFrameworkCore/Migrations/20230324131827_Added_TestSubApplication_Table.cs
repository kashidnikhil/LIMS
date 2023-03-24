using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Added_TestSubApplication_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestSubApplication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMOEF = table.Column<bool>(type: "bit", nullable: false),
                    IsNABL = table.Column<bool>(type: "bit", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSubApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestSubApplication_SubApplications_SubApplicationId",
                        column: x => x.SubApplicationId,
                        principalTable: "SubApplications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestSubApplication_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestVariable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Variable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestVariable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestVariable_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestSubApplication_SubApplicationId",
                table: "TestSubApplication",
                column: "SubApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSubApplication_TestId",
                table: "TestSubApplication",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariable_TestId",
                table: "TestVariable",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestSubApplication");

            migrationBuilder.DropTable(
                name: "TestVariable");
        }
    }
}
