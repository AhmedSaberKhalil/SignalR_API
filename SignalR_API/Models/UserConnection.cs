using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models
{
    public class UserConnection
    {

        [Key]
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
    }
}
