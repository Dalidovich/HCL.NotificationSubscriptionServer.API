using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCL.NotificationSubscriptionServer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class chengeColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "article_id",
                table: "notifications",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "article_id",
                table: "notifications",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");
        }
    }
}
