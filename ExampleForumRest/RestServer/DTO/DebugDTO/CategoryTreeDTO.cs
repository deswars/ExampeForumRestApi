using RestServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestServer.DTO.DebugDTO
{
    public class CategoryTreeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TopicStatuses Status { get; set; }
        public IList<TopicTreeDTO> Topics { get; set; }
        public static CategoryTreeDTO ToDTO(Category category)
        {
            var categoryDTO = new CategoryTreeDTO
            {
                Id = category.Id,
                Name = category.Name,
                Status = category.Status
            };
            categoryDTO.Topics = category.Topics.Select(x => TopicTreeDTO.ToDTO(x)).ToList();
//            categoryDTO.Topics = context.Topics.Where(x => x.Id == categoryDTO.Id).Select(x => TopicTreeDTO.ToDTO(x, context)).ToList();
            return categoryDTO;
        }
    }
}
