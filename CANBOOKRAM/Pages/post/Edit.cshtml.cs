using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CANBOOKRAM.Models;
using System.Security.Claims;

namespace CANBOOKRAM.Pages.post
{
    public class EditModel : PageModel
    {
        private readonly CANBOOKRAM.Models.CANBOOKRAMContext _context;

        public EditModel(CANBOOKRAM.Models.CANBOOKRAMContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserPost UserPost { get; set; } = default!;

        [BindProperty]
        public MessagePictureFile? FileUpload { get; set; }

        public byte[] Picture { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.UserPosts == null)
            {
                return NotFound();
            }

            var userpost =  await _context.UserPosts.FirstOrDefaultAsync(m => m.Id == id);
            if (userpost == null)
            {
                return NotFound();
            }
            UserPost = userpost;
            if (UserPost.Picture != null)
            {
                Picture = UserPost.Picture;
            }
            else
            {
                //Read Image File into Image object.
                string path = "./wwwroot/images/1x1.png";
                var memoryStream = new MemoryStream();
                using (var stream = System.IO.File.OpenRead(path))
                {
                    await new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)).CopyToAsync(memoryStream);
                    Picture = memoryStream.ToArray();
                };
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
           

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                UserPost.UserId = userId;

                if (FileUpload.FormFile != null)
                {
                    var memoryStream = new MemoryStream();
                    await FileUpload.FormFile.CopyToAsync(memoryStream);
                    UserPost.Picture = memoryStream.ToArray();
                }
                else
                {
                    UserPost.Picture = Picture;
                }
                
                UserPost.Message = UserPost.Message;
                UserPost.Timestamp = DateTime.Now;
                _context.UserPosts.Update(UserPost);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPostExists((int)UserPost.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserPostExists(int id)
        {
          return (_context.UserPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
