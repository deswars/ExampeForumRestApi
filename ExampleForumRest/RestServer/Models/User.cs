using System.Collections.Generic;

namespace RestServer.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PsText { get; set; }
        public UserStatuses Status;
        public IList<Message> Messages { get; set; } = new List<Message>();
    }
}
