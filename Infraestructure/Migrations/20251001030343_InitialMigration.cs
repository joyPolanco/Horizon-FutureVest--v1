using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstimatedRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimatedRate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MacroeconomicIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HigherIsBetter = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeightFactor = table.Column<decimal>(type: "decimal(18,2)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroeconomicIndicator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EconomicIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    MacroeconomicIndicatorId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EconomicIndicator_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EconomicIndicator_MacroeconomicIndicator_MacroeconomicIndicatorId",
                        column: x => x.MacroeconomicIndicatorId,
                        principalTable: "MacroeconomicIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimulationMacroIndicator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RealMacroIndicatorId = table.Column<int>(type: "int", nullable: false),
                    WeightFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationMacroIndicator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulationMacroIndicator_MacroeconomicIndicator_RealMacroIndicatorId",
                        column: x => x.RealMacroIndicatorId,
                        principalTable: "MacroeconomicIndicator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EstimatedRate",
                columns: new[] { "Id", "MaxRate", "MinRate" },
                values: new object[] { 1, 15m, 2m });

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsoCode",
                table: "Country",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EconomicIndicator_CountryId",
                table: "EconomicIndicator",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EconomicIndicator_MacroeconomicIndicatorId",
                table: "EconomicIndicator",
                column: "MacroeconomicIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationMacroIndicator_RealMacroIndicatorId",
                table: "SimulationMacroIndicator",
                column: "RealMacroIndicatorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EconomicIndicator");

            migrationBuilder.DropTable(
                name: "EstimatedRate");

            migrationBuilder.DropTable(
                name: "SimulationMacroIndicator");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "MacroeconomicIndicator");
        }
    }
}
