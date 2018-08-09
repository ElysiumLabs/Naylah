using System;

namespace Naylah.Domain
{
    public static class EntityBaseExtensions
    {
        public static string GenerateId(this IEntity<string> entity, bool force = false)
        {
            if ((string.IsNullOrEmpty(entity.Id)) || (force))
            {
                entity.Id = Guid.NewGuid().ToString("D").ToUpper();
                entity.UpdateCreatedAt();
            }

            return entity.Id;
        }

        public static void UpdateCreatedAt(this IModifiableEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        public static void UpdateUpdateAt(this IModifiableEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}