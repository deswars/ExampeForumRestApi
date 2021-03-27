using System;
using RestServer.Models;

namespace RestServer.DTO.DebugDTO
{
    public class MessageTreeDTO
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public MessageStatuses Status { get; set; }
        public long AuthorId { get; set; }

        public static MessageTreeDTO ToDTO(Message message)
        {
            return new MessageTreeDTO
            {
                Id = message.Id,
                Text = message.Text,
                Created = message.Created,
                Modified = message.Modified,
                Status = message.Status,
                AuthorId = message.AuthorId
            };
        }

    }
}
