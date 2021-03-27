using System.Collections.Generic;

namespace RestServer.Models
{
    public class Topic
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public ICollection<Message> Messages { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
