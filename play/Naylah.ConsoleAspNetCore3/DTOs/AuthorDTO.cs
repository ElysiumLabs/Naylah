using System;
using System.Collections.Generic;

namespace Naylah.ConsoleAspNetCore.DTOs
{
    public class AuthorBaseResponse : IEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AuthorRequest : AuthorBaseResponse
    {
        public DateTime BirthDate { get; set; }
    }

    // TODO: needs review on object mappings
    public class AuthorResponse : AuthorBaseResponse
    {
        public ICollection<BookResponse> Books { get; set; }
    }
}