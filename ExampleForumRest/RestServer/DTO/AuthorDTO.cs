using RestServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RestServer.DTO
{
    public class AuthorDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string PsText { get; set; }

        public static AuthorDTO ToDTO(User user)
        {
            return new AuthorDTO
            {
                Id = user.Id,
                Name = user.Name,
                PsText = user.PsText
            };
        }
    }
}
