using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyReviewSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Concepts_ConceptId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_StudyStructures_StudyStructureId",
                table: "FlashcardSets");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "ReviewSchedules");

            migrationBuilder.DropTable(
                name: "SmsMessages");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "StudyStructures");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Relationships",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedForReview",
                table: "Flashcards",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Flashcards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlashcardReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlashcardId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardReviews_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardReviews_FlashcardId_ReviewedAt",
                table: "FlashcardReviews",
                columns: new[] { "FlashcardId", "ReviewedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Concepts_ConceptId",
                table: "Flashcards",
                column: "ConceptId",
                principalTable: "Concepts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_StudyStructures_StudyStructureId",
                table: "FlashcardSets",
                column: "StudyStructureId",
                principalTable: "StudyStructures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Concepts_ConceptId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_StudyStructures_StudyStructureId",
                table: "FlashcardSets");

            migrationBuilder.DropTable(
                name: "FlashcardReviews");

            migrationBuilder.DropColumn(
                name: "IsMarkedForReview",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Flashcards");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StudyStructures",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Relationships",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlashcardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConfidenceLevel = table.Column<float>(type: "REAL", nullable: false),
                    CurrentInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    FailureCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NextReviewAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stage = table.Column<int>(type: "INTEGER", nullable: false),
                    SuccessCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewSchedules_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ElksMessageId = table.Column<string>(type: "TEXT", nullable: true),
                    From = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    To = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "CreatedAt", "Title" },
                values: new object[] { 1, "Welcome to this workshop!", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome" });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "Content", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { 1, "Hello {{name}}, your order {{orderId}} has been confirmed. Thank you for your business!", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Order Confirmation", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewSchedules_FlashcardId",
                table: "ReviewSchedules",
                column: "FlashcardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Concepts_ConceptId",
                table: "Flashcards",
                column: "ConceptId",
                principalTable: "Concepts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_StudyStructures_StudyStructureId",
                table: "FlashcardSets",
                column: "StudyStructureId",
                principalTable: "StudyStructures",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
