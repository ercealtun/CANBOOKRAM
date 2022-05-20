using System.ComponentModel.DataAnnotations;

namespace CANBOOKRAM.Models
{
    public class MessagePictureFile
    {
        [Display(Name = "Picture")]
        public IFormFile? FormFile { get; set; }
    }
}
