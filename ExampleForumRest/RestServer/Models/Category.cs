using System.Collections.Generic;

namespace RestServer.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public IList<Topic> Topics { get; set; } = new List<Topic>();
    }
}
