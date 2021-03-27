using RestServer.Models;
using System;

namespace RestServer.DTO
{
    public class MessageReadDTO
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public MessageStatuses Status { get; set; }

        public long TopicId { get; set; }

        public long AuthorId { get; set; }

        public static MessageReadDTO ToDTO(Message message)
        {
            return new MessageReadDTO { 
                Id = message.Id, 
                Text = message.Text, 
                Created = message.Created, 
                Modified = message.Modified, 
                Status = message.Status, 
                TopicId = message.TopicId, 
                AuthorId = message.AuthorId 
            };
        }
    }
}
