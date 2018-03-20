using System;

namespace Naylah.Core.Entities
{
    public interface IEntityBase
    {
        DateTimeOffset? CreatedAt { get; set; }
        bool Deleted { get; set; }
        string Id { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        string Version { get; set; }
    }

    public static class EntityBaseExtensions
    {
        public static string GenerateId(this IEntityBase entity, bool force = false)
        {
            if ((string.IsNullOrEmpty(entity.Id)) || (force))
            {
                entity.Id = Guid.NewGuid().ToString("D").ToUpper();
                entity.UpdateCreatedAt();
            }

            return entity.Id;
        }

        public static void UpdateCreatedAt(this IEntityBase entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        public static void UpdateUpdateAt(this IEntityBase entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    public abstract class EntityBase : IEntityBase
    {
        public EntityBase()
        {
        }

        public EntityBase(bool generateId = false)
        {
            if (generateId)
            {
                this.GenerateId();
            }
        }

        public string Id { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public string Version { get; set; }
    }
}