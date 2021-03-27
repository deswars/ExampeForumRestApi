using RestServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RestServer.DTO
{
    public class CategoryDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public TopicStatuses Status { get; set; }

        public static CategoryDTO ToDTO(Category category)
        {
            return new CategoryDTO { 
                Id = category.Id, 
                Name = category.Name, 
                Status = category.Status 
            };
        }

        public static Category FromDTO(CategoryDTO categoryDTO)
        {
            return new Category { 
                Id = categoryDTO.Id, 
                Name = categoryDTO.Name, 
                Status = categoryDTO.Status 
            };
        }

        public static void UpdateFromDTO(CategoryDTO categoryDTO, Category category)
        {
            category.Name = categoryDTO.Name;
            category.Status = categoryDTO.Status;
        }
    }
}
