using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApi.Models.Migrations
{
    /// <inheritdoc />
    public partial class ImprovingManyToManyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApplications_AspNetUsers_UserId",
                table: "UserApplications");

            migrationBuilder.DropIndex(
                name: "IX_UserApplications_ApplicationId",
                table: "UserApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppGuid",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserApplications",
                table: "UserApplications",
                columns: new[] { "ApplicationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplications_AspNetUsers_UserId",
                table: "UserApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApplications_AspNetUsers_UserId",
                table: "UserApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserApplications",
                table: "UserApplications");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserApplications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppGuid",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplications_ApplicationId",
                table: "UserApplications",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplications_AspNetUsers_UserId",
                table: "UserApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
