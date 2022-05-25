using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CANBOOKRAM.Data;
using CANBOOKRAM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CANBOOKRAM.Pages
{
    public class AddRatingModel : PageModel
    {
        private readonly CANBOOKRAMContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public string Rate { get; set; }

        [BindProperty]
        public bool isAlreadyRated { get; set; }


        [BindProperty]
        public string RatedUSERID { get; set; }

        public AddRatingModel(CANBOOKRAMContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet(string ratedUserID)
        {
            RatedUSERID = ratedUserID;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var userRating = new UserRating();



            userRating.Rating = Convert.ToInt32(Rate);
            userRating.Userid = RatedUSERID;
            userRating.Whorated = userId;

            await _context.UserRatings.AddAsync(userRating);
            await _context.SaveChangesAsync();

            return RedirectToPage("./RateUser");
        }
    }
}
