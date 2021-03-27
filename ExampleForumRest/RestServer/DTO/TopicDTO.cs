using RestServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RestServer.DTO
{
    public class TopicDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public TopicStatuses Status { get; set; }
        [Required]
        public long CategoryId { get; set; }

        public static TopicDTO ToDTO(Topic topic)
        {
            return new TopicDTO { 
                Id = topic.Id, 
                Name = topic.Name, 
                Status = topic.Status, 
                CategoryId = topic.CategoryId };
        }

        public static Topic FromDTO(TopicDTO topicDTO)
        {
            return new Topic { 
                Id = topicDTO.Id, 
                Name = topicDTO.Name, 
                Status = topicDTO.Status, 
                CategoryId = topicDTO.CategoryId 
            };
        }

        public static void UpdateFromDTO(TopicDTO topicDTO, Topic topic)
        {
            topic.Name = topicDTO.Name;
            topic.Status = topicDTO.Status;
            topic.CategoryId = topicDTO.CategoryId;
        }
    }
}
