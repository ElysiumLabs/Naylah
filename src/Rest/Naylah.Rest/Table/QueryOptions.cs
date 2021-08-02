namespace Naylah.Rest.Table
{
    public class QueryOptions
    {
        public string Filter { get; set; }
        public string Order { get; set; }   
        public int? Top { get; set; }
        public int? Skip { get; set; }
    }

    
}
