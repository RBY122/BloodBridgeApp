using BloodBridge.Data;
using BloodBridge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodBridge.Pages.Dashboard
{
    public class ChatForumModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Donor> _userManager;

        public ChatForumModel(AppDbContext context, UserManager<Donor> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public NewPostModel NewPost { get; set; } = new NewPostModel();

        public List<ChatPost> Posts { get; set; } = new List<ChatPost>();

        public Donor? CurrentDonor { get; set; }


        public string FullName => CurrentDonor?.FullName ?? "Guest";

        public void OnGet()
        {
            // Get current donor
            var userId = _userManager.GetUserId(User);
            CurrentDonor = _context.Donors.FirstOrDefault(d => d.Id == userId);

            Posts = _context.Chatposts.OrderByDescending(p => p.Timestamp).ToList();
        }

        public IActionResult OnPostMessage()
        {
            var userId = _userManager.GetUserId(User);
            CurrentDonor = _context.Donors.FirstOrDefault(d => d.Id == userId);

            if (!ModelState.IsValid)
            {
                Posts = _context.Chatposts.ToList();
                return Page();
            }


                 var donor = _context.Donors.FirstOrDefault(d => d.Id == _userManager.GetUserId(User));

                var newEntry = new ChatPost
                {
                    UserName = donor?.FullName ?? "Guest",
                    Message = NewPost.Message,
                    BloodGroup = donor?.BloodGroup ?? "Unknown",
                    Timestamp = DateTime.UtcNow
                };



            _context.Chatposts.Add(newEntry);
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
