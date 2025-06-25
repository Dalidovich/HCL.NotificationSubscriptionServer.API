using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCL.NotificationSubscriptionServer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    pk_notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.pk_notification_id);
                });

            migrationBuilder.CreateTable(
                name: "relationships",
                columns: table => new
                {
                    pk_relationship_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_master_id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_slave_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relationship_status = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_relationships", x => x.pk_relationship_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_pk_notification_id",
                table: "notifications",
                column: "pk_notification_id");

            migrationBuilder.CreateIndex(
                name: "IX_relationships_pk_relationship_id",
                table: "relationships",
                column: "pk_relationship_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "relationships");
        }
    }
}
