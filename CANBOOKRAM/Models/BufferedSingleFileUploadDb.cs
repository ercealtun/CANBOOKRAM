using System.ComponentModel.DataAnnotations;

namespace CANBOOKRAM.Models
{
    public class BufferedSingleFileUploadDb
    {
        private readonly CANBOOKRAMContext _context;
        [Required(ErrorMessage = "Profile Picture is required.")]
        [Display(Name = "Profile Picture")]
        public IFormFile? FormFile { get; set; }
    }
}
