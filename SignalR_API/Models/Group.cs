using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
