using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Shared.Infrastructure.Database.Migrations;

public partial class AddSpotifyId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SpotifyId",
            table: "Songs",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SpotifyId",
            table: "RoundItems",
            type: "TEXT",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SpotifyId",
            table: "Songs");

        migrationBuilder.DropColumn(
            name: "SpotifyId",
            table: "RoundItems");
    }
} 