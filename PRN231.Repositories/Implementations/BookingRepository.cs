using PRN231.Models;
using PRN231.Repositories.Interfaces;
using PRN231.Repository.Interfaces;

namespace PRN231.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IGenericRepository<Booking> _genericRepository;

        public BookingRepository(IGenericRepository<Booking> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Task<Booking> Add(Booking entity)
        {
            return _genericRepository.Add(entity);
        }

        public Task<Booking> Delete(params int[] keys)
        {
            return (_genericRepository.Delete(keys));
        }

        public Task<Booking> Get(params int[] keys)
        {
            return _genericRepository.Get(keys);
        }

        public Task<IEnumerable<Booking>> GetAll(params Func<IQueryable<Booking>, IQueryable<Booking>>[] includes)
        {
            return _genericRepository.GetAll(includes);
        }

        public List<Booking> GetAllBookingsByStatus(string status, params Func<IQueryable<Booking>, IQueryable<Booking>>[] includes)
        {
            return _genericRepository.GetAll(includes).Result.Where(b => b.Status.Equals(status)).ToList();
        }

        public Task<Booking> Update(Booking entity)
        {
            return _genericRepository.Update(entity);
        }
    }
}
