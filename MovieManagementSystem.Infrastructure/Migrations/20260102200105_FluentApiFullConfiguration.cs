using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FluentApiFullConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Studios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Movies",
                type: "numeric(3,1)",
                precision: 3,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Movies",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                table: "MovieDetails",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "MovieDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "MovieDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "MovieDetails",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_Studios_Name",
                table: "Studios",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ReleaseDate_Rating",
                table: "Movies",
                columns: new[] { "ReleaseDate", "Rating" });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Name",
                table: "Actors",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Studios_Name",
                table: "Studios");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ReleaseDate_Rating",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Title",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Genres_Name",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Actors_Name",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Studios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Movies",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,1)",
                oldPrecision: 3,
                oldScale: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                table: "MovieDetails",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "MovieDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "MovieDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "MovieDetails",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
