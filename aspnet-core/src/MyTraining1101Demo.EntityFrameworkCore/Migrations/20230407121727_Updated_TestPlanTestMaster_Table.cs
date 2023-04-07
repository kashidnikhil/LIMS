using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTraining1101Demo.Migrations
{
    public partial class Updated_TestPlanTestMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestPlanTestMaster_StandardReference_StandardReferenceId",
                table: "TestPlanTestMaster");

            migrationBuilder.RenameColumn(
                name: "StandardReferenceId",
                table: "TestPlanTestMaster",
                newName: "TestPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_TestPlanTestMaster_StandardReferenceId",
                table: "TestPlanTestMaster",
                newName: "IX_TestPlanTestMaster_TestPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestPlanTestMaster_TestPlan_TestPlanId",
                table: "TestPlanTestMaster",
                column: "TestPlanId",
                principalTable: "TestPlan",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestPlanTestMaster_TestPlan_TestPlanId",
                table: "TestPlanTestMaster");

            migrationBuilder.RenameColumn(
                name: "TestPlanId",
                table: "TestPlanTestMaster",
                newName: "StandardReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_TestPlanTestMaster_TestPlanId",
                table: "TestPlanTestMaster",
                newName: "IX_TestPlanTestMaster_StandardReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestPlanTestMaster_StandardReference_StandardReferenceId",
                table: "TestPlanTestMaster",
                column: "StandardReferenceId",
                principalTable: "StandardReference",
                principalColumn: "Id");
        }
    }
}
