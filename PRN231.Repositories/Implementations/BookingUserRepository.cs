using PRN231.Models;
using PRN231.Repositories.Interfaces;
using PRN231.Repository.Interfaces;

namespace PRN231.Repositories.Implementations
{
    public class BookingUserRepository : IBookingUserRepository
    {
        private readonly IGenericRepository<BookingUser> _genericRepository;

        public BookingUserRepository(IGenericRepository<BookingUser> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public Task<BookingUser> Add(BookingUser entity)
        {
            return _genericRepository.Add(entity);
        }

        public Task<BookingUser> Delete(params int[] keys)
        {
            return (_genericRepository.Delete(keys));
        }

        public Task<BookingUser> Get(params int[] keys)
        {
           return _genericRepository.Get(keys);
        }

        public Task<IEnumerable<BookingUser>> GetAll(params Func<IQueryable<BookingUser>, IQueryable<BookingUser>>[] includes)
        {
            return _genericRepository.GetAll(includes);
        }

        public Task<BookingUser> Update(BookingUser entity)
        {
            return _genericRepository.Update(entity);
        }
    }
}
