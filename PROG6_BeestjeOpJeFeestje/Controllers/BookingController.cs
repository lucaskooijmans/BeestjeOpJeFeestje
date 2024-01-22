using AutoMapper;
using BeestjeOpJeFeestje.Domain;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Repositories.Interfaces;
using PROG6_BeestjeOpJeFeestje.ViewModels;
using Microsoft.AspNetCore.Authorization;
using PROG6_BeestjeOpJeFeestje.ViewModels.Validation;

namespace PROG6_BeestjeOpJeFeestje.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BookingController(IBookingRepository bookingRepository, IMapper mapper, IAnimalRepository animalRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _animalRepository = animalRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllBookings();
            return View(bookings);
        }
        
        public async Task<IActionResult> IndexForUser(string user)
        {
            var bookings = await _bookingRepository.GetAllBookingsForUser(user);
            return View("Index", bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdOrNull(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var booking = await _bookingRepository.GetBookingByIdOrNull(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingRepository.DeleteBooking(id);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult SetDate()
        {
            _animalRepository.SetFiltersToDefault();
            return View();
        }
        
        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetDate([Bind("Date")] BookingDateVM booking)
        {
            if (ModelState.IsValid)
            {
                BookingSession bookingSession = new BookingSession
                {
                    Date = booking.Date
                };
                HttpContext.Session.Set("BookingSession", bookingSession);
                return RedirectToAction("SelectAnimals");
            }

            return View(booking);
        }
        

        [HttpGet]
        public async Task<IActionResult> SelectAnimals()
        {
            BookingSession? bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            DateTime bookingDate = bookingSession.Date;
            
            User? user = null;
            if(HttpContext.User.Identity?.Name != null)
            {
                user = await _userRepository.GetUserByEmailOrNull(HttpContext.User.Identity.Name);
            }
            
            // Retrieve available animals from the database
            _animalRepository.ExcludeUnavailable = true;
            _animalRepository.ExcludePenguin = Validator.IsWeekend(bookingDate);
            _animalRepository.ExcludeSnow = Validator.ExcludeSnow(bookingDate);
            _animalRepository.ExcludeDesert = Validator.ExcludeDesert(bookingDate);
            _animalRepository.ExcludeVip = user?.MemberCard != MemberCard.Platinum;
            List<Animal> models = (await _animalRepository.GetAllAvailableAnimals(bookingDate)).ToList();
            List<AnimalVM> animals = _mapper.Map<List<AnimalVM>>(models);
            
            BookingSummaryVM bookingSummary = new BookingSummaryVM
            {
                Date = bookingDate,
            };
            BookingAnimalsVM viewModel = new BookingAnimalsVM { Animals = animals, BookingSummary = bookingSummary };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectAnimals(BookingAnimalsVM model)
        {
            BookingSession? bookingSession = null;
            if (!ModelState.IsValid)
            {
                bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
                if (bookingSession == null) return RedirectToAction("Index", "Home");
                model.BookingSummary = new BookingSummaryVM
                {
                    Date = bookingSession.Date,
                };
                return View(model);
            }

            User? user = null;
            if(HttpContext.User.Identity?.Name != null)
            {
                user = await _userRepository.GetUserByEmailOrNull(HttpContext.User.Identity.Name);
            }

            bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            
            List<Animal> selectedAnimals = model.Animals.Where(a => a.Selected).Select(a => _mapper.Map<Animal>(a)).ToList();

            if (selectedAnimals.Count == 0)
            {
                ModelState.AddModelError("Animals", "Select at least one animal");
                return View(model);
            }

            if (selectedAnimals.Count > 3 && user == null)
            {
                ModelState.AddModelError("Animals", "You can only select 3 animals");
                return View(model);
            }
            if(selectedAnimals.Count > 4 && user?.MemberCard == MemberCard.Silver)
            {
                ModelState.AddModelError("Animals", "You can only select less then 4 animals");
                return View(model);
            }
            if(selectedAnimals.Count > 5 && user?.MemberCard != MemberCard.Gold && user?.MemberCard != MemberCard.Platinum)
            {
                ModelState.AddModelError("Animals", "You can only select less then 5 animals");
                return View(model);
            }

            if (selectedAnimals.Any(a => a.Name is "Leeuw" or "IJsbeer") &&
                selectedAnimals.Any(a => a.Type == AnimalTypes.Boerderij.ToString()))
            {
                ModelState.AddModelError("Animals", "You can't select a farm animal with a lion or polar bear");
                return View(model);
            }
            
            bookingSession.Animals = selectedAnimals;
           
            
            HttpContext.Session.Set("BookingSession", bookingSession);
            
            // Save selected animals in session or hidden field
            // Redirect to the next step
            return RedirectToAction("EnterContactInfo");
        }

        [HttpGet]
        public IActionResult EnterContactInfo()
        {
            
            BookingSession? bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
            
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            
            BookingSummaryVM bookingSummary = new BookingSummaryVM()
            {
                Date = bookingSession.Date,
                SelectedAnimals = _mapper.Map<List<AnimalVM>>(bookingSession.Animals)
            };
            BookingContactInfoVM viewModel = new BookingContactInfoVM(){BookingSummary = bookingSummary};
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EnterContactInfo(BookingContactInfoVM model)
        {
            
            BookingSession? bookingSession = null;
            if (!ModelState.IsValid)
            {
                bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
                if (bookingSession == null) return RedirectToAction("Index", "Home");
                model.BookingSummary = new BookingSummaryVM
                {
                    Date = bookingSession.Date,
                    SelectedAnimals = _mapper.Map<List<AnimalVM>>(bookingSession.Animals)
                };
                return View(model);
            }
            
            bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
            
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            
            bookingSession.FirstName = model.FirstName;
            bookingSession.InBetween = model.InBetween;
            bookingSession.LastName = model.LastName;
            bookingSession.Address = model.Address;
            bookingSession.Email = model.Email;
            
            
            HttpContext.Session.Set("BookingSession", bookingSession);
            // Save contact information in session or hidden field
            // Redirect to the next step or display the booking summary
            return RedirectToAction("BookingSummary");
        }
        [Authorize]
        public async Task<IActionResult> UseAccount()
        {
            BookingSession? bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");  
            if (bookingSession == null) return RedirectToAction("Index", "Home");

            if (HttpContext.User.Identity?.Name == null) return RedirectToAction("EnterContactInfo", "Booking");
            User? user = await _userRepository.GetUserByEmailOrNull(HttpContext.User.Identity.Name);
            if (user == null) return RedirectToAction("EnterContactInfo", "Booking");
            
            bookingSession.FirstName = user.FirstName;
            bookingSession.InBetween = user.InBetween;
            bookingSession.LastName = user.LastName;
            bookingSession.Address = user.Address;
            bookingSession.Email = user.Email;
            bookingSession.UserId = user.Id;
            HttpContext.Session.Set("BookingSession", bookingSession);
                // Retrieve user information from the database
                // Display the booking summary
                return RedirectToAction("BookingSummary");
        }

        [HttpGet]
        public IActionResult BookingSummary()
        {
            
            BookingSession? bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");  
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            
            DiscountCalculator discountCalculator = new DiscountCalculator();
            
            bookingSession.Discounts = discountCalculator.CalculateTotalDiscount(bookingSession); 
            bookingSession.Price = discountCalculator.CalculateTotalPrice(bookingSession);

            
            BookingSummaryVM viewModel = new BookingSummaryVM
            {
                Date = bookingSession.Date,
                FirstName = bookingSession.FirstName!,
                InBetween = bookingSession.InBetween!,
                LastName = bookingSession.LastName!,
                Address = bookingSession.Address!,
                Email = bookingSession.Email!,
                Price = bookingSession.Price!,
                Discounts = bookingSession.Discounts,
                SelectedAnimals = _mapper.Map<List<AnimalVM>>(bookingSession.Animals)
            };
            // Retrieve saved information from session or hidden fields
            // Display the booking summary
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Accept()
        {
            
            BookingSession? bookingSession =  HttpContext.Session.Get<BookingSession>("BookingSession");
            if (bookingSession == null) return RedirectToAction("Index", "Home");
            
            await _bookingRepository.AddBooking(new Booking
            {
                Date = bookingSession.Date,
                FirstName = bookingSession.FirstName!,
                InBetween = bookingSession.InBetween!,
                LastName = bookingSession.LastName!,
                Address = bookingSession.Address!,
                Email = bookingSession.Email!,
                Animals = bookingSession.Animals,
                UserId = bookingSession.UserId
            });
            
            HttpContext.Session.Remove("BookingSession");

            return RedirectToAction(nameof(Index), "Home");
        }
        
        public IActionResult Cancel()
        {
            HttpContext.Session.Remove("BookingSession");
            return RedirectToAction("Index", "Home");
        }
    }
}
