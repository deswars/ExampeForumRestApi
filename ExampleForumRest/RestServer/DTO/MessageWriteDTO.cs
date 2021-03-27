using RestServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RestServer.DTO
{
    public class MessageWriteDTO
    {
        public long Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public MessageStatuses Status { get; set; }

        [Required]
        public long TopicId { get; set; }
        [Required]
        public long AuthorId { get; set; }

        public static Message FromDTO(MessageWriteDTO messageDTO)
        {
            return new Message
            {
                Id = messageDTO.Id,
                Text = messageDTO.Text,
                Status = messageDTO.Status,
                TopicId = messageDTO.TopicId,
                AuthorId = messageDTO.AuthorId
            };
        }

        public static void UpdateFromDTO(MessageWriteDTO messageDTO, Message message)
        {
            message.Text = messageDTO.Text;
            message.Status = messageDTO.Status;
            message.TopicId = messageDTO.TopicId;
            message.AuthorId = messageDTO.AuthorId;
        }
    }
}
