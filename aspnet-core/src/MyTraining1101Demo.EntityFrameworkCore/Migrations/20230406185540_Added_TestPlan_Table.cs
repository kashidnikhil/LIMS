using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Added_TestPlan_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StandardReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_TestPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestPlan_Applications_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestPlan_StandardReference_StandardReferenceId",
                        column: x => x.StandardReferenceId,
                        principalTable: "StandardReference",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestPlanTestMaster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StandardReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_TestPlanTestMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestPlanTestMaster_StandardReference_StandardReferenceId",
                        column: x => x.StandardReferenceId,
                        principalTable: "StandardReference",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestPlanTestMaster_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestPlan_ApplicationsId",
                table: "TestPlan",
                column: "ApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPlan_StandardReferenceId",
                table: "TestPlan",
                column: "StandardReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPlanTestMaster_StandardReferenceId",
                table: "TestPlanTestMaster",
                column: "StandardReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPlanTestMaster_TestId",
                table: "TestPlanTestMaster",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestPlan");

            migrationBuilder.DropTable(
                name: "TestPlanTestMaster");
        }
    }
}
