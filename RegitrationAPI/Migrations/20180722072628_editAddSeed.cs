using Microsoft.EntityFrameworkCore.Migrations;

namespace RegitrationAPI.Migrations
{
    public partial class editAddSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole<string>");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "157c73c9-a9b8-4561-8304-cc6a521b07bd", "12afc995-f277-4757-abc1-737621cea813", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bba471f0-6123-4e6c-8662-9daf182f8630", "32563de3-6df5-479d-8be0-fe47fb0a806e", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fdbdf21a-50f4-45f3-a831-952688bdf22d", "8d1f7fd3-2394-4516-83b1-d683b7b96905", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "157c73c9-a9b8-4561-8304-cc6a521b07bd", "12afc995-f277-4757-abc1-737621cea813" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "bba471f0-6123-4e6c-8662-9daf182f8630", "32563de3-6df5-479d-8be0-fe47fb0a806e" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "fdbdf21a-50f4-45f3-a831-952688bdf22d", "8d1f7fd3-2394-4516-83b1-d683b7b96905" });

            migrationBuilder.CreateTable(
                name: "IdentityRole<string>",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole<string>", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole<string>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "74c66695-52f0-49c0-98e7-af7d97f0f337", "784f42ef-8e42-4297-abb3-1de7765879f8", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "IdentityRole<string>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eada3064-3eb3-4735-ab44-522295a5f548", "145f866d-1aca-4eee-88a8-8ad55e0104f2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "IdentityRole<string>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "17805555-1935-4fff-a559-71b7ce371d8e", "da9579c3-b3b4-4c70-ab81-dd048e5b3223", "User", "USER" });
        }
    }
}
