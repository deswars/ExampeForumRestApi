using System.ComponentModel;

namespace RestServer.Models
{
    public enum UserStatuses
    {
        [Description("Active")]
        Created = 0,

        [Description("Deleted")]
        Edited = 1,
    }
}
