using System.ComponentModel.DataAnnotations;

namespace SignalR_API.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }  // Primary key

        public int GroupId { get; set; }
        public string Name { get; set; }
    }
}
