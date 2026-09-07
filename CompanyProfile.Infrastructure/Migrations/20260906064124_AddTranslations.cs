using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProfile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CompanyInformation");

            migrationBuilder.DropColumn(
                name: "Mission",
                table: "CompanyInformation");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CompanyInformation");

            migrationBuilder.DropColumn(
                name: "Vision",
                table: "CompanyInformation");

            migrationBuilder.CreateTable(
                name: "CompanyInfoTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Vision = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Mission = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfoTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyInfoTranslations_CompanyInformation_CompanyInfoId",
                        column: x => x.CompanyInfoId,
                        principalTable: "CompanyInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTranslations_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMemberTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamMemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberTranslations_Team_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInfoTranslations_CompanyInfoId_Language",
                table: "CompanyInfoTranslations",
                columns: new[] { "CompanyInfoId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTranslations_ServiceId_Language",
                table: "ServiceTranslations",
                columns: new[] { "ServiceId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberTranslations_TeamMemberId_Language",
                table: "TeamMemberTranslations",
                columns: new[] { "TeamMemberId", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyInfoTranslations");

            migrationBuilder.DropTable(
                name: "ServiceTranslations");

            migrationBuilder.DropTable(
                name: "TeamMemberTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Team",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Team",
                type: "TEXT",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Team",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Services",
                type: "TEXT",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CompanyInformation",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mission",
                table: "CompanyInformation",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CompanyInformation",
                type: "TEXT",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vision",
                table: "CompanyInformation",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
