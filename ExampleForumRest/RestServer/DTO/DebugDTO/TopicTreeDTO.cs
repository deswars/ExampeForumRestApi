using System.Collections.Generic;
using System.Linq;
using RestServer.Models;

namespace RestServer.DTO.DebugDTO
{
    public class TopicTreeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public IList<MessageTreeDTO> Messages { get; set; }

        public static TopicTreeDTO ToDTO(Topic topic)
        {
            var topicDTO = new TopicTreeDTO
            {
                Id = topic.Id,
                Name = topic.Name,
                Status = topic.Status,
            };
            topicDTO.Messages = topic.Messages.Select(x => MessageTreeDTO.ToDTO(x)).ToList();
//            topicDTO.Messages = context.Messages.Where(x => x.TopicId == topicDTO.Id).Select(x => MessageTreeDTO.ToDTO(x)).ToList();
            return topicDTO;
        }
    }
}
