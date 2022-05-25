using CANBOOKRAM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CANBOOKRAM.Pages.friend
{
    [Authorize]
    public class AddFriendModel : PageModel
    {
        private readonly CANBOOKRAMContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty]
        public List<SelectListItem> Users { get; set; }
        [BindProperty]
        public List<SelectListItem> FriendshipRequests { get; set; }
        [BindProperty]
        public List<SelectListItem> FriendList { get; set; }
        [BindProperty]
        public string Current_User { get; set; }
        public Friends friendRequest { get; set; }
        
        public AddFriendModel(CANBOOKRAMContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class FriendRequestModel
        {
            public int FriendshipId { get; set; }
            public string Email { get; set; }
            public string OwnerId { get; set; }
            public string SenderId { get; set; }
            public bool Approved { get; set; }
        }

        public void OnGet()
        {
            Current_User = User.Identity.Name;

            Users = _userManager.Users.ToList()
                .Select(a => new SelectListItem { Text = a.UserName, Value = a.Id })
                .Where(a => a.Text != Current_User)
                .OrderBy(s => s.Text).ToList();

            var current_User = _userManager.FindByEmailAsync(Current_User);
            FriendshipRequests = _context.Friends
                .Select(a => new SelectListItem { Text = _userManager.FindByIdAsync(a.UserId).Result.Email, Value = a.Id.ToString()}).ToList()
                .Where(x => _context.Friends.FindAsync(Int32.Parse(x.Value)).Result.FriendId.Equals(current_User.Result.Id)
                    && !_context.Friends.FindAsync(Int32.Parse(x.Value)).Result.Approved).ToList();

            FriendList = _context.Friends
                .Select(a => new SelectListItem { Text = _userManager.FindByIdAsync(a.UserId).Result.Email, Value = a.Id.ToString() }).ToList()
                .Where(x => _context.Friends.FindAsync(Int32.Parse(x.Value)).Result.FriendId.Equals(current_User.Result.Id)
                    && _context.Friends.FindAsync(Int32.Parse(x.Value)).Result.Approved).ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            var current_User = await _userManager.GetUserAsync(HttpContext.User);
            friendRequest = new Friends();
            friendRequest.FriendId = Request.Form["receiver"].ToString();
            friendRequest.UserId = current_User.Id;
            friendRequest.Approved = false;

            await  _context.Friends.AddAsync(friendRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OnPostEdit()
        {
            int friendshipId = Int32.Parse(Request.Form["requests"].ToString());
            var friendRequest = await _context.Friends.FindAsync(friendshipId);
            if (friendRequest != null)
                friendRequest.Approved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OnPostView()
        {
            int friendshipId = Int32.Parse(Request.Form["friends"].ToString());
            var friendRequest = await _context.Friends.FindAsync(friendshipId);
            if (friendRequest != null)
                friendRequest.Approved = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
