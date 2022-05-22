using CANBOOKRAM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CANBOOKRAM.Pages.Friend
{
    [Authorize]
    public class AddFriendModel : PageModel
    {
        private readonly ILogger<AddFriendModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CANBOOKRAMContext _context;

        [BindProperty]
        public List<SelectListItem> Users { get; set; }

        [BindProperty]
        public string MyUser { get; set; }
        public AddFriendModel(ILogger<AddFriendModel> logger, UserManager<IdentityUser> userManager, CANBOOKRAMContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGet()
        {
            var friendRequest = new Friends();
            friendRequest.FriendId = "1";
            friendRequest.UserId = "2";
            friendRequest.Approved = false;
            await _context.Friends.AddAsync(friendRequest);
            await _context.SaveChangesAsync();

            //get all the users from the database
            Users = _userManager.Users.ToList()
                .Select(a => new SelectListItem { Text = a.UserName, Value = a.UserName })
                .OrderBy(s => s.Text).ToList();

            //get logged in user name
            MyUser = User.Identity.Name;

        }

        //public async Task OnPostAsync()
        //{
        //    var friendRequest = new Friends();
        //    friendRequest.FriendId = "1";
        //    friendRequest.UserId = "2";
        //    friendRequest.Approved = false;
        //    await _context.Friends.AddAsync(friendRequest);
        //    await _context.SaveChangesAsync();
        //}
    }
}
