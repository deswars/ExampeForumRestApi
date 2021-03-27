using System;

namespace RestServer.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public MessageStatuses Status { get; set; }

        public long TopicId { get; set; }
        public Topic Topic { get; set; }

        public long AuthorId { get; set; }
        public User Author { get; set; }
    }
}
