using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportCatalog.Migrations
{
    public partial class AddRptSetupToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rpt_setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RptCode = table.Column<string>(maxLength: 60, nullable: false),
                    RptDesc = table.Column<string>(maxLength: 100, nullable: false),
                    Pos = table.Column<string>(maxLength: 3, nullable: true),
                    Filename = table.Column<string>(maxLength: 50, nullable: false),
                    RptParam = table.Column<string>(maxLength: 40, nullable: true),
                    Version = table.Column<int>(nullable: true),
                    Disabled = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rpt_setup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rpt_setup");
        }
    }
}
