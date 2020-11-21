using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadlyFetcher
{
    public class Publications
    {
        public class Root
        {
            public List<Content> Content { get; set; }
        }

        public class Content
        {
            public string Id { get; set; }
            public string Publication { get; set; }
            public Uri Imageurl { get; set; }
            public string Title { get; set; }
        }
    }

    public class Issues
    {
        public partial class Root
        {
            public List<Content> Content { get; set; }
            public string Description { get; set; }
        }

        public partial class Content
        {
            public string Id { get; set; }
            public string Publication { get; set; }
            public Uri Imageurl { get; set; }
            public string Title { get; set; }
            [JsonProperty("publish_date")]
            public DateTime PublishDate { get; set; }
            public string Issue { get; set; }
        }
    }

    public class Pages
    {
        public List<string> Content { get; set; }
        public bool Success { get; set; }
    }

    public class SingleIssue
    {
        public string Id { get; set; }
        public string Publication { get; set; }
        public Uri Imageurl { get; set; }
        public string Title { get; set; }
        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }
        public string Issue { get; set; }
    }

}
