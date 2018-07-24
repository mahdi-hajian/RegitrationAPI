using Microsoft.EntityFrameworkCore.Migrations;

namespace RegitrationAPI.Migrations
{
    public partial class addMoreRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7112ba80-7d03-4098-aa5c-26fb4ab164b7", "37124343-ef4c-4926-9d1d-e4530798d255", "Leader", "LEADER" },
                    { "3206af8a-b615-4c2a-9b85-59b7bf6c8126", "1a7c788d-f658-4505-9a09-f183bc33e5e3", "Admin", "ADMIN" },
                    { "de54640b-c37e-4476-afbe-2267fc0b07f7", "fb6fa6ae-e658-487e-a9c6-7e4a6a6dbe24", "Manager", "MANAGER" },
                    { "8bbed163-0944-4a8c-91ef-8e52e8c2563a", "c7e57ffe-4f28-4427-b527-4a629686c29a", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "3206af8a-b615-4c2a-9b85-59b7bf6c8126", "1a7c788d-f658-4505-9a09-f183bc33e5e3" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "7112ba80-7d03-4098-aa5c-26fb4ab164b7", "37124343-ef4c-4926-9d1d-e4530798d255" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "8bbed163-0944-4a8c-91ef-8e52e8c2563a", "c7e57ffe-4f28-4427-b527-4a629686c29a" });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "de54640b-c37e-4476-afbe-2267fc0b07f7", "fb6fa6ae-e658-487e-a9c6-7e4a6a6dbe24" });

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
    }
}
