﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApi.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingModelForUsersIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "App",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "App",
                table: "AspNetUsers");
        }
    }
}
