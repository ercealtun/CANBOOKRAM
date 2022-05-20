using System;
using System.Collections.Generic;

namespace CANBOOKRAM.Models
{
    public partial class UserPost
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
