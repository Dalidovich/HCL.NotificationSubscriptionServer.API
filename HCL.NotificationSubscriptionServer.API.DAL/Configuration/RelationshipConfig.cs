using HCL.NotificationSubscriptionServer.API.DAL.Configuration.DataType;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCL.NotificationSubscriptionServer.API.DAL.Configuration
{
    public class RelationshipConfig : IEntityTypeConfiguration<Relationship>
    {
        public const string Table_name = "relationships";

        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });
            builder.HasIndex(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("pk_relationship_id");

            builder.Property(e => e.AccountMasterId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("account_master_id");

            builder.Property(e => e.AccountSlaveId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("account_slave_id");

            builder.Property(e => e.Status)
                   .HasColumnType(EntityDataTypes.Smallint)
                   .HasColumnName("relationship_status");
        }
    }
}
