using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportCatalog.Migrations
{
    public partial class AddReportDataToRptSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ReportData",
                table: "rpt_setup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportData",
                table: "rpt_setup");
        }
    }
}
