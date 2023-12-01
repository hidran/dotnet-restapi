using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PmsApi.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    PriorityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PriorityName = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.PriorityId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProjectCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.CategoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProjectCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PriorityId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Priority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priority",
                        principalColumn: "PriorityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TaskAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileData = table.Column<byte[]>(type: "blob", nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.InsertData(
                table: "Priority",
                columns: new[] { "PriorityId", "PriorityName" },
                values: new object[,]
                {
                    { 1, "Alta" },
                    { 2, "Media" },
                    { 3, "Bassa" },
                    { 4, "Urgente" }
                });

            migrationBuilder.InsertData(
                table: "ProjectCategories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Software Development" },
                    { 2, "Web Development" },
                    { 3, "Database Systems" },
                    { 4, "System Integration" },
                    { 5, "Testing" },
                    { 6, "Maintenance" },
                    { 7, "Research and Development" },
                    { 8, "IT Support" },
                    { 9, "Marketing Campaigns" },
                    { 10, "Product Launch" },
                    { 11, "Event Management" },
                    { 12, "Content Creation" },
                    { 13, "Social Media Management" },
                    { 14, "Customer Relations" },
                    { 15, "Sales Strategies" },
                    { 16, "Market Research" },
                    { 17, "Financial Planning" },
                    { 18, "Budget Management" },
                    { 19, "Legal Compliance" },
                    { 20, "Environmental Projects" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Manager" },
                    { 3, "Developer" },
                    { 4, "Designer" },
                    { 5, "Tester" },
                    { 6, "HR" },
                    { 7, "Sales" },
                    { 8, "Marketing" },
                    { 9, "Support" },
                    { 10, "Customer Service" },
                    { 11, "Finance" },
                    { 12, "Legal" },
                    { 13, "Public Relations" },
                    { 14, "Operations" },
                    { 15, "IT" },
                    { 16, "Product Manager" },
                    { 17, "Data Analyst" },
                    { 18, "Business Analyst" },
                    { 19, "Executive" },
                    { 20, "Intern" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "In Attesa" },
                    { 2, "In Corso" },
                    { 3, "Completato" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Password", "RoleId", "Username" },
                values: new object[,]
                {
                    { 1, "emma.stone@mail.com", "Emma", "Stone", "pass123", 1, "user1" },
                    { 2, "liam.smith@mail.com", "Liam", "Smith", "pass123", 2, "user2" },
                    { 3, "olivia.jones@mail.com", "Olivia", "Jones", "pass123", 3, "user3" },
                    { 4, "noah.brown@mail.com", "Noah", "Brown", "pass123", 4, "user4" },
                    { 5, "jacob.williams@mail.com", "Jacob", "Williams", "pass123", 5, "user5" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CategoryId", "Description", "EndDate", "ManagerId", "ProjectName", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, "Descrizione del Progetto Alfa", new DateOnly(2021, 6, 30), 1, "Progetto Alfa", new DateOnly(2021, 1, 1) },
                    { 2, 2, "Descrizione del Progetto Beta", new DateOnly(2021, 12, 31), 2, "Progetto Beta", new DateOnly(2021, 7, 1) },
                    { 3, 3, "Descrizione del Progetto Gamma", new DateOnly(2022, 6, 30), 3, "Progetto Gamma", new DateOnly(2022, 1, 1) },
                    { 4, 4, "Descrizione del Progetto Delta", new DateOnly(2022, 12, 31), 4, "Progetto Delta", new DateOnly(2022, 7, 1) },
                    { 5, 5, "Descrizione del Progetto Epsilon", new DateOnly(2023, 6, 30), 5, "Progetto Epsilon", new DateOnly(2023, 1, 1) },
                    { 6, 6, "Descrizione del Progetto Zeta", new DateOnly(2023, 12, 31), 5, "Progetto Zeta", new DateOnly(2023, 7, 1) },
                    { 7, 7, "Descrizione del Progetto Eta", new DateOnly(2024, 6, 30), 4, "Progetto Eta", new DateOnly(2024, 1, 1) },
                    { 8, 8, "Descrizione del Progetto Theta", new DateOnly(2024, 12, 31), 3, "Progetto Theta", new DateOnly(2024, 7, 1) },
                    { 9, 9, "Descrizione del Progetto Iota", new DateOnly(2025, 6, 30), 5, "Progetto Iota", new DateOnly(2025, 1, 1) },
                    { 10, 10, "Descrizione del Progetto Kappa", new DateOnly(2025, 12, 31), 4, "Progetto Kappa", new DateOnly(2025, 7, 1) }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "AssignedUserId", "CreatedDate", "Description", "DueDate", "PriorityId", "ProjectId", "StatusId", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateOnly(2021, 1, 1), "Descrizione Task 1", new DateOnly(2021, 6, 30), 1, 1, 1, "Task 1" },
                    { 2, 2, new DateOnly(2021, 2, 1), "Descrizione Task 2", new DateOnly(2021, 7, 30), 2, 2, 2, "Task 2" },
                    { 3, 3, new DateOnly(2021, 3, 1), "Descrizione Task 3", new DateOnly(2021, 8, 30), 3, 3, 3, "Task 3" }
                });

            migrationBuilder.InsertData(
                table: "TaskAttachments",
                columns: new[] { "Id", "FileData", "FileName", "TaskId" },
                values: new object[,]
                {
                    { 1, null, "Attachment1.pdf", 1 },
                    { 2, null, "Attachment2.pdf", 1 },
                    { 3, null, "Attachment3.pdf", 2 },
                    { 4, null, "Attachment4.pdf", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Priority_PriorityName",
                table: "Priority",
                column: "PriorityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectName",
                table: "Projects",
                column: "ProjectName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Status_StatusName",
                table: "Status",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_TaskId",
                table: "TaskAttachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedUserId",
                table: "Tasks",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PriorityId",
                table: "Tasks",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAttachments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "ProjectCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
