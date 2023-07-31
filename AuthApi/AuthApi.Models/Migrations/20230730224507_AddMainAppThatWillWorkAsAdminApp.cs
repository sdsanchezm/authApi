using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApi.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddMainAppThatWillWorkAsAdminApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("adqwq",
                                        columns: new[]
                                        {
                                            nameof(Application.Application.ApplicationName),
                                            nameof(Application.Application.CreationDate),
                                            nameof(Application.Application.Enabled),
                                            nameof(Application.Application.AppGuid),
                                        },
                                        values: new object[,]
                                        {
                                            { "AdminApp", DateTime.Now, true, "9DDBE0C6-84A5-434E-8B8F-39750DF2C392" }
                                        });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
