using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CANBOOKRAM.Data;
using CANBOOKRAM.Model;
using CANBOOKRAM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CANBOOKRAM.Pages
{
    [Authorize]
    public class RateUserModel : PageModel
    {
        private readonly CANBOOKRAMContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RateUserModel(CANBOOKRAMContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string RatedUser { get; set; }

        
        public List<RatedUser> UserRatingsList { get; set; }

        public async Task ListRatings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId

            //Get all users without the logged-in user
            var AllUsers = await (from u in _context.AspNetUsers
                                  where u.Id != userId
                                  select u).ToListAsync();
            //Get all ratings
            var AllRatings = await (from r in _context.UserRatings
                                    select r).ToListAsync();

            //Intialize and fill [BindPropoerty] UserRatingList containing user information and average ratings 
            UserRatingsList = new();

            foreach (var item in AllUsers)
            {
                //if user has roles (in our demo application only admins have a role), do not add this user to the list
                var user = await _userManager.FindByIdAsync(item.Id);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count >= 1)
                {
                    continue;
                }
                //find all ratings of the current user in foreach loop
                List<int?> userrate = await (from x in _context.UserRatings where x.Userid == item.Id select x.Rating).ToListAsync();
                var userating = new RatedUser();
                if (userrate.Count >= 1)
                {
                    //calculate current user's average rating
                    double avg = CalculateRate.GetRating(userrate);
                    //avg = Math.Round(avg, 1); //round to 1 decimal place

                    //set values
                    userating.AvgRating = avg.ToString();
                    userating.UserId = item.Id;
                    userating.Name = item.UserName;
                    userating.PhoneNumber = item.PhoneNumber;
                    userating.Email = item.Email;
                    UserRatingsList.Add(userating);
                }
                else  //if user has no ratings do NOT calculate average
                {
                    //set values
                    userating.AvgRating = "0";
                    userating.UserId = item.Id;
                    userating.Name = item.UserName;
                    userating.PhoneNumber = item.PhoneNumber;
                    userating.Email = item.Email;
                    UserRatingsList.Add(userating);
                }
            }
        }

        public async Task OnGetAsync()
        {
            //List users and their average ratings
            await ListRatings();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //List users and their average ratings in each post
            await ListRatings();

            return Page();
        }

    }
}
