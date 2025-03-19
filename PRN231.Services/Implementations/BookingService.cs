using Microsoft.EntityFrameworkCore;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repositories.Interfaces;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;

namespace PRN231.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingUserRepository _bookingUserRepository;

        public BookingService(IBookingRepository bookingRepository,
            IBookingUserRepository bookingUserRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingUserRepository = bookingUserRepository;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAll(
                    query => query.Include(b => b.Subject)
                                  .Include(b => b.Level)
                                  .Include(b => b.BookingUsers)
                                  .Include(b => b.Schedules));

            return bookings.Select(booking => new BookingDto
            {
                Id = booking.Id,
                SubjectName = booking.Subject.Name,
                SubjectId = booking.Subject.Id,
                LevelName = booking.Level.LevelName,
                StartDate = booking.StartDate,
                NumOfSlots = booking.NumOfSlots,
                PricePerSlot = booking.PricePerSlot,
                PaymentMethod = booking.PaymentMethod,
                Description = booking.Description,
                Status = booking.Status,
                CreatedDate = booking.CreatedDate,
                UpdatedDate = booking.UpdatedDate,
                Schedules = booking.Schedules.Select(s => new ScheduleDTO {
                    Id = s.Id,
                    BookingId = s.BookingId,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    Duration = s.Duration,
                    Status = s.Status
                }),
                BookingUsers = booking.BookingUsers.Select(bu => new BookingUserDTO
                {
                    UserId = bu.UserId,
                    Role = bu.Role,
                    Description = bu.Description,
                    Status = bu.Status
                })
            });
        }

        public async Task<Booking> GetBooking(int bookingId)
        {
           return await _bookingRepository.Get(bookingId);
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsByStatus(string status)
        {
            return GetAllBookings().Result.Where(b => b.Status.Equals(status));
        }

        public async Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            Booking booking = new Booking
            {
                SubjectId = createBookingRequest.SubjectId,
                LevelId = createBookingRequest.LevelId,
                PricePerSlot = createBookingRequest.PricePerSlot,
                StartDate = createBookingRequest.StartDate,
                NumOfSlots = createBookingRequest.NumOfSlots,
                PaymentMethod = PaymentMethodConstant.VNPAY,
                Description = createBookingRequest.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingStatusConstant.PENDING
            };

            try
            {
                Booking addedBooking = await _bookingRepository.Add(booking);

                BookingUser bookingUser = new BookingUser
                {
                    UserId = createBookingRequest.UserId,
                    BookingId = addedBooking.Id,
                    Role = RoleEnum.STUDENT,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Status = StatusConstant.ACTIVE
                };

                BookingUser savedBookingUser = await _bookingUserRepository.Add(bookingUser);

                CreateBookingResponse bookingResponse = new CreateBookingResponse
                {
                    Id = addedBooking.Id,
                    SubjectId = addedBooking.SubjectId,
                    LevelId = addedBooking.LevelId,
                    UserId = savedBookingUser.UserId,
                    StartDate = addedBooking.StartDate,
                    Role = savedBookingUser.Role,
                    Description = addedBooking.Description,
                    NumOfSlots = addedBooking.NumOfSlots,
                    PricePerSlot = (decimal)addedBooking.PricePerSlot,
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest)
        {
            try
            {
                Booking existingBooking = _bookingRepository.GetAll().Result
                    .FirstOrDefault(b => b.Id ==  updateBookingRequest.BookingId);
                existingBooking.SubjectId = updateBookingRequest.SubjectId;
                existingBooking.LevelId = updateBookingRequest.LevelId;
                existingBooking.PricePerSlot = updateBookingRequest.PricePerSlot;
                existingBooking.NumOfSlots = updateBookingRequest.NumOfSlots;
                existingBooking.StartDate = updateBookingRequest.StartDate;
                existingBooking.PaymentMethod = updateBookingRequest.PaymentMethod;
                existingBooking.Description = updateBookingRequest.Description;
                existingBooking.Status = updateBookingRequest.Status;

                Booking updatedBooking = await _bookingRepository.Update(existingBooking);

                UpdateBookingResponse bookingResponse = new UpdateBookingResponse
                {
                    SubjectId = updatedBooking.SubjectId,
                    LevelId = updatedBooking.LevelId,
                    PricePerSlot = (decimal)updatedBooking.PricePerSlot,
                    StartDate = updatedBooking.StartDate,
                    NumOfSlots = updatedBooking.NumOfSlots,
                    PaymentMethod = updatedBooking.PaymentMethod,
                    Status = updatedBooking.Status
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

         public async Task<IEnumerable<BookingUserDTO>> GetAllTutorsByBooking(int bookingId)
        {
            var bookingUsers = await _bookingUserRepository.GetAll(
                query => query.Where(bu => bu.BookingId == bookingId && bu.Role == RoleEnum.TUTOR)
            );

            return bookingUsers.Select(bu => new BookingUserDTO
            {
                UserId = bu.UserId,
                BookingId = bu.BookingId,
                Role = bu.Role,
                Status = bu.Status,
                Description = bu.Description
            });
        }

        public async Task<bool> ApplyToBooking(int userId, int bookingId)
        {
            BookingUser bookingUser = new BookingUser
            {
                UserId = userId,
                BookingId = bookingId,
                Role = RoleEnum.TUTOR,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingUserStatusConstant.APPLIED
            };

            try
            {
                await _bookingUserRepository.Add(bookingUser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AcceptTutor(int bookingId, int tutorId)
        {
            Booking booking = await _bookingRepository.Get(bookingId);
            booking.Status = BookingStatusConstant.APPROVED;

            await _bookingRepository.Update(booking);

            var bookingUsers = await _bookingUserRepository.GetAll(
                query => query.Where(bu => bu.BookingId == bookingId && bu.Role == RoleEnum.TUTOR)
            );


            foreach (var bu in bookingUsers)
            {
                if (bu.UserId == tutorId)
                {
                    bu.Status = BookingUserStatusConstant.APPROVED;
                }
                else
                {
                    bu.Status = BookingUserStatusConstant.REJECTED;
                }

                await _bookingUserRepository.Update(bu);
            }

            return true;
        }
        public async Task<bool> CancelApplication(int userId, int bookingId)
        {
            var bookingUser = await _bookingUserRepository.GetAll();
            var bookingUserCancel = bookingUser.FirstOrDefault(bu => bu.UserId == userId && bu.BookingId == bookingId);

            if (bookingUser != null)
            {
                await _bookingUserRepository.Delete(bookingUserCancel.Id);
                return true;
            }

            return false;
        }
    }
}
