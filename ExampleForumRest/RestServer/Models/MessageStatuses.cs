using System.ComponentModel;

namespace RestServer.Models
{
    public enum MessageStatuses
    {
        [Description("Created")]
        Created = 1,

        [Description("Edited")]
        Edited = 2,

        [Description("Deleted")]
        Deleted = 4
    }
}
