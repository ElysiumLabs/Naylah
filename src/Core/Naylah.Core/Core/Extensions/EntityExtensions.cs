using System;

namespace Naylah
{
    public static class EntityBaseExtensions
    {
        public static string GenerateId(this IEntity<string> entity, bool force = false)
        {
            if ((string.IsNullOrEmpty(entity.Id)) || (force))
            {
                entity.Id = Guid.NewGuid().ToString("D").ToUpper();
            }

            return entity.Id;
        }

        public static void UpdateCreatedAt(this IModifiable entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        public static void UpdateUpdateAt(this IModifiable entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}