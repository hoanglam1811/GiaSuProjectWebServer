using PRN231.Models;
using PRN231.Repository.Interfaces;

namespace PRN231.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        List<Booking> GetAllBookingsByStatus(string status, params Func<IQueryable<Booking>, IQueryable<Booking>>[] includes);
    }
}
