using System;
using System.Collections.Generic;

namespace CANBOOKRAM.Models
{
    public partial class UserRating
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string? Userid { get; set; }
        public string? Whorated { get; set; }
    }
}
