using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }  
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
    }
}
