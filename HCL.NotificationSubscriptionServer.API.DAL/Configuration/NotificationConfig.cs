using HCL.NotificationSubscriptionServer.API.DAL.Configuration.DataType;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.DAL.Configuration
{
    public class NotificationConfig:IEntityTypeConfiguration<Notification>
    {
        public const string Table_name = "notifications";

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });
            builder.HasIndex(e => e.Id);

            builder.Property(e => e.Id)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("pk_notification_id");

            builder.Property(e => e.RelationshipId)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("fk_relationship_id");

            builder.Property(e => e.ArticleId)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("article_id");
        }
    }
}
