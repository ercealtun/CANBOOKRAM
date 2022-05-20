using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CANBOOKRAM.Models
{
    public partial class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
