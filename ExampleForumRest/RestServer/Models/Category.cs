using System.Collections.Generic;

namespace RestServer.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
}
