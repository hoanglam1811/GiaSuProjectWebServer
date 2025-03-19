using AutoMapper;
using PRN231.Models;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;

namespace PRN231.Services.Implementations
{
    public class GenericService<T, D> : IGenericService<T, D> where T : class
    {
        protected readonly IGenericRepository<T> _genericRepository;
        protected readonly IMapper _mapper;

        public GenericService(IMapper mapper, IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<T> Add(D dto)
        {
            try{
                var entity = _mapper.Map<T>(dto);
                await _genericRepository.Add(entity);
                return entity;
            }
            catch (DbUpdateException ex)
            {
                Exception innerEx = ex;
                while(innerEx != null)
                {
                    if (innerEx is SqlException sqlEx)
                    {
                        // Check the SQL error number for specific constraint violations
                        if (sqlEx.Number == 547) // Foreign key constraint violation
                        {
                            throw new Exception("Foreign key not found");
                        }
                        else if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Unique constraint violation
                        {
                            throw new Exception("Id is duplicated");
                        }
                        else
                        {
                            // Handle other types of SQL exceptions
                            throw new Exception("An error occurred while saving changes. Please try again later.");
                        }
                    }
                    innerEx = innerEx.InnerException;
                }
                throw new Exception("An error occurred while saving changes. Please try again later.");
            }
        }

        public async Task<T> Delete(params int[] keys)
        {
            var entity = await _genericRepository.Get(keys);
            if (entity == null) return null;
            await _genericRepository.Delete(keys);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = await _genericRepository.GetAll();
            return entities;
        }

        public async Task<T> Get(params int[] keys)
        {
            var entity = await _genericRepository.Get(keys);     
            return entity;
        }

        public async Task<T> Update(D dto)
        {
            var entity = _mapper.Map<T>(dto);
            await _genericRepository.Update(entity);
            return entity;
        }
    }
}
