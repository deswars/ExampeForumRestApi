using System.Collections.Generic;

namespace RestServer.Models
{
    public class Topic
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public IList<Message> Messages { get; set; } = new List<Message>();

        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
