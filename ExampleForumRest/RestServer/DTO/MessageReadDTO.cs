using RestServer.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace RestServer.DTO
{
    public class MessageReadDTO
    {
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        [Required]
        public MessageStatuses Status { get; set; }

        [Required]
        public long TopicId { get; set; }

        [Required]
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
