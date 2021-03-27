using RestServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestServer.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PsText { get; set; }
        public UserStatuses Status { get; set; }

        public static UserDTO ToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                PsText = user.PsText,
                Status = user.Status
            };
        }
        public static User FromDTO(UserDTO userDTO)
        {
            return new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                PsText = userDTO.PsText,
                Status = userDTO.Status
            };
        }

        public static void UpdateFromDTO(UserDTO userDTO, User user)
        {
            user.Id = userDTO.Id;
            user.Name = userDTO.Name;
            user.PsText = userDTO.PsText;
            user.Status = userDTO.Status;
        }
    }
}
