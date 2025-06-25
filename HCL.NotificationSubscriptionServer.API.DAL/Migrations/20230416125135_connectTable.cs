using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCL.NotificationSubscriptionServer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class connectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "notifications",
                newName: "fk_relationship_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_fk_relationship_id",
                table: "notifications",
                column: "fk_relationship_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_relationships_fk_relationship_id",
                table: "notifications",
                column: "fk_relationship_id",
                principalTable: "relationships",
                principalColumn: "pk_relationship_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notifications_relationships_fk_relationship_id",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_fk_relationship_id",
                table: "notifications");

            migrationBuilder.RenameColumn(
                name: "fk_relationship_id",
                table: "notifications",
                newName: "account_id");
        }
    }
}
