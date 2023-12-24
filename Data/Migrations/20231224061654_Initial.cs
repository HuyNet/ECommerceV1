using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imagePath",
                table: "ProductImages",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "IsDedault",
                table: "ProductImages",
                newName: "IsDefault");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("30658b70-9f61-40b8-a6a8-c0e6ef85b2d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9e006118-4b8a-4e19-b0d1-fd7d1213b354", "AQAAAAIAAYagAAAAEJgb/Ei+0bo6odxgRDYMjPQXYV7Kz84W+wOw1yYdFfNQUJtQWdBOY4LaXkS+57lqUQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ProductImages",
                newName: "imagePath");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "ProductImages",
                newName: "IsDedault");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("30658b70-9f61-40b8-a6a8-c0e6ef85b2d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cd0e44ad-4291-47cc-bca2-159f7cd92351", "AQAAAAIAAYagAAAAEAYj1DLsyvJw3a4eUA2z1CnEIAHgXDjDReggrBHUZOu4oC4Rrxfkj9gXxkan0LNYmQ==" });
        }
    }
}
