using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;

namespace PRN231.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookings();
        Task<Booking> GetBooking(int bookingId);
        Task<IEnumerable<BookingDto>> GetAllBookingsByStatus(string status);
        Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest);
        Task<bool> ApplyToBooking(int userId, int bookingId);
        Task<bool> AcceptTutor(int bookingId, int tutorId);
        Task<IEnumerable<BookingUserDTO>> GetAllTutorsByBooking(int bookingId);
        Task<bool> CancelApplication(int userId, int bookingId);
    }
}
