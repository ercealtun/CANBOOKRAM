using System;
using System.Collections.Generic;

namespace CANBOOKRAM.Models
{
    public partial class UserProfile
    {
        //public int? Id { get; set; }
        public string? UserId { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
