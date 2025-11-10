using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SimulationMacroIndicator_MacroeconomicIndicator_RealMacroIndicatorId",
                table: "SimulationMacroIndicator");

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationMacroIndicator_MacroeconomicIndicator_RealMacroIndicatorId",
                table: "SimulationMacroIndicator",
                column: "RealMacroIndicatorId",
                principalTable: "MacroeconomicIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SimulationMacroIndicator_MacroeconomicIndicator_RealMacroIndicatorId",
                table: "SimulationMacroIndicator");

            migrationBuilder.AddForeignKey(
                name: "FK_SimulationMacroIndicator_MacroeconomicIndicator_RealMacroIndicatorId",
                table: "SimulationMacroIndicator",
                column: "RealMacroIndicatorId",
                principalTable: "MacroeconomicIndicator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
