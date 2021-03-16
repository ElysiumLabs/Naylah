using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Naylah.ConsoleAspNetCore.DTOs;

namespace Naylah.ConsoleAspNetCore.Entities
{
    public class Author :
        IEntity<string>,
        IModifiable,
        IEntityUpdate<AuthorRequest>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Book> Books { get; set; }
        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Version { get; set; }
        public bool Deleted { get; set; }

        internal static void EntityConfigure(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(m => m.Books)
                .WithOne(o => o.Author)
                .HasForeignKey(fk => fk.AuthorId);
        }

        public void UpdateFrom(AuthorRequest source, EntityUpdateOptions options = null)
        {
            this.Name = source.Name;
            this.BirthDate = source.BirthDate;
        }
    }
}