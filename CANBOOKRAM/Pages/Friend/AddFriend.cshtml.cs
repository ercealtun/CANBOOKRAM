using CANBOOKRAM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string Current_User { get; set; }
        public IEnumerable<FriendRequestModel> FriendshipRequests { get; set; }
        public IEnumerable<FriendRequestModel> FriendList { get; set; }
        public Friends friendRequest { get; set; }
        
        public AddFriendModel(CANBOOKRAMContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class FriendRequestModel
        {
            public string Email { get; set; }
            public string OwnerId { get; set; }
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
                .Select(a => new FriendRequestModel { OwnerId = a.FriendId, Approved = a.Approved, Email = _userManager.FindByIdAsync(a.UserId).Result.Email })
                .Where(x => x.OwnerId.Equals(current_User.Result.Id) && !x.Approved).ToList();

            FriendList = _context.Friends
                .Select(a => new FriendRequestModel { OwnerId = a.FriendId, Approved = a.Approved, Email = _userManager.FindByIdAsync(a.UserId).Result.Email })
                .Where(x => x.OwnerId.Equals(current_User.Result.Id) && x.Approved).ToList();
        }

        public async Task<IActionResult> OnPostAcceptRequest(Friends friendRequest)
        {


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OnPost(Friends friendRequest)
        {
            var current_User = await _userManager.GetUserAsync(HttpContext.User);
            friendRequest.FriendId = Request.Form["receiver"].ToString();
            friendRequest.UserId = current_User.Id;
            friendRequest.Approved = false;

            await  _context.Friends.AddAsync(friendRequest);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
