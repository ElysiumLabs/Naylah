using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Rest.Table
{
    [JsonObject()]
    public class PageResult<TModel> : PageResult, IEnumerable<TModel>
    {
        public QueryOptions Query { get; set; }

        [JsonProperty("items")]
        public IEnumerable<TModel> Items { get; set; } = new List<TModel>();

        public IEnumerator<TModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    public class PageResult
    {
        [JsonProperty("nextPageLink")]
        public Uri NextPageLink { get; set; }

        [JsonProperty("count")]
        public long? Count { get; set; }

    }
}
