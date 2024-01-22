using AutoMapper;
using Domain.Models;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PROG6_BeestjeOpJeFeestje.Controllers;
using PROG6_BeestjeOpJeFeestje.ViewModels;

namespace PROG6_BeestjeOpJeFeestje.Test
{
    public class BookingControllerTests
    {
        
        private readonly Mock<IBookingRepository> _bookingsRepository = new();
        private readonly Mock<IAnimalRepository> _animalRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IMapper> _mapper = new();

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnViewWithBooking()
        {
            // Arrange
            const int bookingId = 1;
            _bookingsRepository.Setup(repo => repo.GetBookingByIdOrNull(bookingId))
                .ReturnsAsync(new Booking { Id = bookingId });

            var controller = new BookingController(_bookingsRepository.Object, _mapper.Object, _animalRepository.Object, _userRepository.Object);

            // Act
            var result = await controller.Delete(bookingId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Booking>(viewResult.ViewData.Model);
            Assert.Equal(bookingId, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_WithValidId_ShouldDeleteBookingAndRedirectToIndex()
        {
            // Arrange
            const int bookingId = 1;
            var controller = new BookingController(_bookingsRepository.Object, _mapper.Object, _animalRepository.Object, _userRepository.Object);

            // Act
            var result = await controller.DeleteConfirmed(bookingId);

            // Assert
            _bookingsRepository.Verify(repo => repo.DeleteBooking(bookingId), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task SetDate_WithValidModelState_ShouldSetBookingSessionAndRedirectToSelectAnimals()
        {
            // Arrange
            var controller = new BookingController(_bookingsRepository.Object, _mapper.Object, _animalRepository.Object, _userRepository.Object);
            var bookingDateVM = new BookingDateVM { Date = DateTime.Now };

            // Act
            var result = controller.SetDate(bookingDateVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("SelectAnimals", redirectToActionResult.ActionName);

            var bookingSession = controller.HttpContext.Session.Get<BookingSession>("BookingSession");
            Assert.NotNull(bookingSession);
            Assert.Equal(bookingDateVM.Date, bookingSession.Date);
        }
//
//         [Fact]
// public async Task SelectAnimals_WithModelStateError_ShouldReturnViewWithError()
// {
//     // Arrange
//     var controller = new BookingController(Mock.Of<IBookingRepository>(), _mapper.Object, _animalRepository.Object, _userRepository.Object);
//     controller.ModelState.AddModelError("Animals", "Select at least one animal");
//
//     // Act
//     var result = await controller.SelectAnimals(new BookingAnimalsVM());
//
//     // Assert
//     var viewResult = Assert.IsType<ViewResult>(result);
//     Assert.True(controller.ModelState.ErrorCount > 0);
//     Assert.False(controller.ModelState.IsValid);
// }
//
// [Fact]
// public async Task EnterContactInfo_WithValidModelState_ShouldSetBookingSessionAndRedirectToBookingSummary()
// {
//     // Arrange
//     var controller = new BookingController(Mock.Of<IBookingRepository>(), _mapper.Object, _animalRepository.Object, _userRepository.Object);
//     var model = new BookingContactInfoVM
//     {
//         FirstName = "John",
//         InBetween = "Doe",
//         LastName = "Smith",
//         Address = "123 Main St",
//         Email = "john@example.com"
//     };
//
//     // Act
//     var result = controller.EnterContactInfo(model);
//
//     // Assert
//     var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
//     Assert.Equal("BookingSummary", redirectToActionResult.ActionName);
//
//     var bookingSession = controller.HttpContext.Session.Get<BookingSession>("BookingSession");
//     Assert.NotNull(bookingSession);
//     Assert.Equal(model.FirstName, bookingSession.FirstName);
//     Assert.Equal(model.InBetween, bookingSession.InBetween);
//     Assert.Equal(model.LastName, bookingSession.LastName);
//     Assert.Equal(model.Address, bookingSession.Address);
//     Assert.Equal(model.Email, bookingSession.Email);
// }
//
// [Fact]
// public async Task UseAccount_WithValidUser_ShouldSetBookingSessionAndRedirectToBookingSummary()
// {
//     // Arrange
//     var user = new User
//     {
//         FirstName = "John",
//         InBetween = "Doe",
//         LastName = "Smith",
//         Address = "123 Main St",
//         Email = "john@example.com",
//         MemberCard = MemberCard.None
//     };
//
//     var userRepositoryMock = new Mock<IUserRepository>();
//     userRepositoryMock.Setup(repo => repo.GetUserByEmailOrNull("john@example.com")).ReturnsAsync(user);
//
//     var controller = new BookingController(Mock.Of<IBookingRepository>(), _mapper.Object, _animalRepository.Object, userRepositoryMock.Object);
//
//     // Act
//     var result = await controller.UseAccount();
//
//     // Assert
//     var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
//     Assert.Equal("BookingSummary", redirectToActionResult.ActionName);
//
//     var bookingSession = controller.HttpContext.Session.Get<BookingSession>("BookingSession");
//     Assert.NotNull(bookingSession);
//     Assert.Equal(user.FirstName, bookingSession.FirstName);
//     Assert.Equal(user.InBetween, bookingSession.InBetween);
//     Assert.Equal(user.LastName, bookingSession.LastName);
//     Assert.Equal(user.Address, bookingSession.Address);
//     Assert.Equal(user.Email, bookingSession.Email);
//     Assert.Equal(user.Id, bookingSession.UserId);
// }
//
// [Fact]
// public void BookingSummary_ShouldReturnViewWithBookingSummaryVM()
// {
//     // Arrange
//     var controller = new BookingController(Mock.Of<IBookingRepository>(), _mapper.Object, _animalRepository.Object, _userRepository.Object);
//
//     var bookingSession = new BookingSession
//     {
//         Date = DateTime.Now,
//         FirstName = "John",
//         InBetween = "Doe",
//         LastName = "Smith",
//         Address = "123 Main St",
//         Email = "john@example.com",
//         Animals = new List<Animal>()
//     };
//
//     var sessionMock = new Mock<ISession>();
//     sessionMock.Setup(s => s.Get<BookingSession>("BookingSession")).Returns(bookingSession);
//
//     controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { Session = sessionMock.Object } };
//
//     // Act
//     var result = controller.BookingSummary();
//
//     // Assert
//     var viewResult = Assert.IsType<ViewResult>(result);
//     var model = Assert.IsAssignableFrom<BookingSummaryVM>(viewResult.ViewData.Model);
//
//     Assert.Equal(bookingSession.Date, model.Date);
//     Assert.Equal(bookingSession.FirstName, model.FirstName);
//     Assert.Equal(bookingSession.InBetween, model.InBetween);
//     Assert.Equal(bookingSession.LastName, model.LastName);
//     Assert.Equal(bookingSession.Address, model.Address);
//     Assert.Equal(bookingSession.Email, model.Email);
//     Assert.Empty(model.SelectedAnimals); // Assuming no animals in the session
// }

    }
}
