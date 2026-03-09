using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlleyCatBarbers.Data;
using AlleyCatBarbers.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AlleyCatBarbers.Models;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.CodeAnalysis.Scripting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;


namespace AlleyCatBarbers.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<BookingsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            IQueryable<Booking> applicationDbContext = _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User);

            //Restrict viewing of bookings to creator if user is not Admin or Staff
            if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
            {
                applicationDbContext = applicationDbContext
                    .Where(b => b.UserId == user.Id);
            }

            return View(await applicationDbContext.ToListAsync());

        }

       

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)   
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,TimeSlot,ServiceId")] BookingViewModel bookingViewModel)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
            }
            else
            {
                _logger.LogInformation("User authenticated: {UserId}", user.Id);
                bookingViewModel.UserId = user.Id;
            }

           

            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    Date = bookingViewModel.Date,
                    TimeSlot = TimeOnly.Parse(bookingViewModel.TimeSlot),
                    ServiceId = bookingViewModel.ServiceId,
                    UserId = user.Id
                };

                _context.Add(booking);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Booking created successfully: {BookingId}", booking.Id);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Type", bookingViewModel.ServiceId);
            
            _logger.LogWarning("ModelState is not valid. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            
            return View(bookingViewModel);
        }


        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }


            // Pass the booking details to the ViewModel
            var bookingViewModel = new BookingViewModel
            {
                Id = booking.Id,
                Date = booking.Date,
                TimeSlot = booking.TimeSlot.ToString("h:mm tt"),
                ServiceId = booking.ServiceId
            };

            // Populate ViewBag with data to prepopulate the dropdowns
            ViewBag.ServiceId = new SelectList(_context.Services, "Id", "Type", booking.ServiceId);

            return View(bookingViewModel);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,TimeSlot,ServiceId")] BookingViewModel bookingViewModel)
        {

            if (id != bookingViewModel.Id)
            {
                return BadRequest();
            }

            var booking = await _context.Bookings.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (booking == null)
            {
                _logger.LogWarning("No booking found with ID {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Booking found: {Booking}", booking);

            if (user == null)
            {
                return NotFound();
            }


            if (string.IsNullOrEmpty(bookingViewModel.TimeSlot))
            {
                ModelState.AddModelError("TimeSlot", "Time slot is required.");
                ViewBag.ServiceId = new SelectList(_context.Services, "Id", "Type", bookingViewModel.ServiceId);
                return View(bookingViewModel);
            }

            

            try
            {
                // Update booking record with new values from ViewModel
                booking.Date = bookingViewModel.Date;
                booking.TimeSlot = TimeOnly.ParseExact(bookingViewModel.TimeSlot, "h:mm tt", null);
                booking.ServiceId = bookingViewModel.ServiceId;

                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception caught : {ex.Message}");
                ViewBag.ServiceId = new SelectList(_context.Services, "Id", "Type", bookingViewModel.ServiceId);
                return View(bookingViewModel);
            }
        
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetAvailableTimeSlots(DateTime date, string currentBookingTimeSlot)
        {
            
            try
            {
                // Get existing bookings for the given date
                var bookings = _context.Bookings
                                       .Where(b => b.Date.Date == date.Date)
                                       //.Select(b => b.TimeSlot)
                                       .ToList();

                // Define all possible time slots
                var allTimeSlots = new List<string>
                {
                    "10:00 AM", "11:00 AM", "12:00 PM", "1:00 PM", "2:00 PM",
                    "3:00 PM", "4:00 PM", "5:00 PM"
                };


                // Convert booked times to strings for comparison
                var bookedTimeSlots = bookings.Select(b => b.TimeSlot.ToString()).ToList();

                // Remove current booking time from BookedTimeSlots so it is still available
                if (!string.IsNullOrEmpty(currentBookingTimeSlot))
                {
                    bookedTimeSlots.Remove(currentBookingTimeSlot);
                }

                // Remove booked time slots from the list of all time slots
                var availableTimeSlots = allTimeSlots.Except(bookedTimeSlots, StringComparer.OrdinalIgnoreCase).ToList();

                _logger.LogInformation("Booked Time Slots: {TimeSlots}", string.Join(", ", bookedTimeSlots));
                _logger.LogInformation("Available Time Slots: {TimeSlots}", string.Join(", ", availableTimeSlots));

                // Return available time slots
                return Ok(availableTimeSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available time slots for date: {date}", date);
                return StatusCode(500, "Internal server error");
            }
        }


    }
}


