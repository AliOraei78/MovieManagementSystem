using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomSqlAndAdvancedSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Custom raw SQL — for example, creating a special index or a function
            migrationBuilder.Sql(@"
        CREATE INDEX IF NOT EXISTS idx_movies_title_trgm 
        ON ""Movies"" USING GIN (""Title"" gin_trgm_ops);
    ");

            migrationBuilder.Sql(@"
        CREATE OR REPLACE FUNCTION update_rating_average()
        RETURNS TRIGGER AS $$
        BEGIN
            UPDATE ""Movies""
            SET ""Rating"" = (
                SELECT AVG(""Rating"")
                FROM ""Movies""
                WHERE ""StudioId"" = NEW.""StudioId""
            )
            WHERE ""StudioId"" = NEW.""StudioId"";
            RETURN NEW;
        END;
        $$ LANGUAGE plpgsql;
    ");

            // Custom trigger
            migrationBuilder.Sql(@"
        CREATE TRIGGER trg_update_rating_avg
        AFTER INSERT OR UPDATE ON ""Movies""
        FOR EACH ROW
        EXECUTE FUNCTION update_rating_average();
    ");

            // Complex HasData (Many-to-Many relationships) — EF Core adds these automatically
            // (They were defined earlier using HasData in the model configuration)
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_update_rating_avg ON \"Movies\";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_rating_average();");
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_movies_title_trgm;");
        }
    }
}
