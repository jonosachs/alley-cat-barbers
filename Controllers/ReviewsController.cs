using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AlleyCatBarbers.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AlleyCatBarbers.Controllers
{
    
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingsController> _logger;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<BookingsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Reviews
        public async Task<IActionResult> List()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id.ToString(), // ReviewViewModel takes string for Id
                    Rating = r.Rating,
                    Comments = r.Comments,
                    FirstName = r.User.FirstName,
                    DateCreated = r.DateCreated

                })
                .ToListAsync();

            // Calculate average rating to nearest 0.5
            var viewModel = new ReviewListViewModel
            {
                Reviews = reviews,
                AverageRating = reviews.Any() ? (Math.Round(reviews.Average(r => r.Rating), 2)) : 0
            };

            return View(viewModel);
        }

        // GET: Reviews/Create
        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(ReviewViewModel reviewViewModel)
        {
            // Remove Id and FirstName properties from the ViewModel as these are set by the Model 
            ModelState.Remove(nameof(reviewViewModel.Id));
            ModelState.Remove(nameof(reviewViewModel.FirstName));

            if (ModelState.IsValid)
            {
                // Get the current User
                var user = await _userManager.GetUserAsync(User);
                
                // Create a new Review with data from ViewModel
                var review = new Review
                {
                    Rating = reviewViewModel.Rating,
                    Comments = reviewViewModel.Comments,
                    UserId = user.Id, // Supply current User Id
                    DateCreated = DateTime.Now
                };

                _context.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            

            return View(reviewViewModel);
        }


        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            
            var reviewViewModel = new ReviewViewModel
            {
                Id = review.Id.ToString(),
                Rating = review.Rating,
                Comments = review.Comments,
                FirstName = review.User.FirstName,
                DateCreated = review.DateCreated

            };

            return View(reviewViewModel);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        
    }

  
}
