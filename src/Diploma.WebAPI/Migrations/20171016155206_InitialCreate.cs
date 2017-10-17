using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diploma.WebAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ProjectTeams");

            migrationBuilder.DropTable("TeamMembers");

            migrationBuilder.DropTable("Projects");

            migrationBuilder.DropTable("Teams");

            migrationBuilder.DropTable("Users");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Teams",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Teams", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Salary = table.Column<decimal>("TEXT", nullable: true),
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BirthDate = table.Column<DateTime>("TEXT", nullable: true),
                    FirstName = table.Column<string>("TEXT", nullable: false),
                    Gender = table.Column<int>("INTEGER", nullable: false),
                    LastName = table.Column<string>("TEXT", nullable: false),
                    MiddleName = table.Column<string>("TEXT", nullable: true),
                    PasswordHash = table.Column<string>("TEXT", nullable: false),
                    Role = table.Column<string>("TEXT", nullable: false),
                    Username = table.Column<string>("TEXT", maxLength: 30, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Projects",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>("INTEGER", nullable: false),
                    DeadlineDate = table.Column<DateTime>("TEXT", nullable: false),
                    Foreit = table.Column<decimal>("TEXT", nullable: false),
                    ManagerId = table.Column<int>("INTEGER", nullable: false),
                    NumberOfMilestones = table.Column<int>("INTEGER", nullable: false),
                    PricePerHour = table.Column<decimal>("TEXT", nullable: false),
                    ProjectType = table.Column<string>("TEXT", nullable: false),
                    Title = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey("FK_Projects_Users_CustomerId", x => x.CustomerId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Projects_Users_ManagerId", x => x.ManagerId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TeamMembers",
                table => new
                {
                    Id = table.Column<int>("INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgrammerId = table.Column<int>("INTEGER", nullable: false),
                    TeamId = table.Column<int>("INTEGER", nullable: false),
                    TeamRole = table.Column<string>("TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey("FK_TeamMembers_Users_ProgrammerId", x => x.ProgrammerId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_TeamMembers_Teams_TeamId", x => x.TeamId, "Teams", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ProjectTeams",
                table => new
                {
                    ProjectId = table.Column<int>("INTEGER", nullable: false),
                    TeamId = table.Column<int>("INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_ProjectTeams",
                        x => new
                        {
                            x.ProjectId,
                            x.TeamId
                        });
                    table.ForeignKey("FK_ProjectTeams_Projects_ProjectId", x => x.ProjectId, "Projects", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_ProjectTeams_Teams_TeamId", x => x.TeamId, "Teams", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_Projects_CustomerId", "Projects", "CustomerId");

            migrationBuilder.CreateIndex("IX_Projects_ManagerId", "Projects", "ManagerId");

            migrationBuilder.CreateIndex("IX_ProjectTeams_TeamId", "ProjectTeams", "TeamId");

            migrationBuilder.CreateIndex("IX_TeamMembers_ProgrammerId", "TeamMembers", "ProgrammerId");

            migrationBuilder.CreateIndex("IX_TeamMembers_TeamId", "TeamMembers", "TeamId");

            migrationBuilder.CreateIndex("IX_Users_Username", "Users", "Username", unique: true);
        }
    }
}
