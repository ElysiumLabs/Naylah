using System;
using Microsoft.EntityFrameworkCore;
using Naylah.ConsoleAspNetCore.DTOs;

namespace Naylah.ConsoleAspNetCore.Entities
{
    public class Book :
        IEntity<string>,
        IModifiable,
        IEntityUpdate<BookRequest>
    {
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public int ReleasedYear { get; set; }
        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Version { get; set; }
        public bool Deleted { get; set; }

        internal static void EntityConfigure(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(p => p.Title)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(p => p.AuthorId)
                .IsRequired();
        }

        public void UpdateFrom(BookRequest source, EntityUpdateOptions options = null)
        {
            this.Title = source.Title;
            this.AuthorId = source.AuthorId;
            this.ReleasedYear = source.ReleasedYear;
        }
    }
}