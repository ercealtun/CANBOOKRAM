using System.ComponentModel.DataAnnotations;

namespace CANBOOKRAM.Models
{
    public class Friends
    {
        [Key]
        //public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FriendId { get; set; }
        public bool Approved { get; set; }
    }
}
