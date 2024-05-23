using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_API.Models
{
    public class UserGroup
    {

        [Key]
        public int GroupId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}
