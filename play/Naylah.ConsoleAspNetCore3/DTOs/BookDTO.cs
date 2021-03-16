namespace Naylah.ConsoleAspNetCore.DTOs
{
    public class BookBaseResponse : IEntity<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int ReleasedYear { get; set; }
    }

    public class BookRequest : IEntity<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int ReleasedYear { get; set; }
        public string AuthorId { get; set; }
    }

    public class BookResponse : BookBaseResponse
    {
        public AuthorBaseResponse Author { get; set; }
    }
}