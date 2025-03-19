namespace PRN231.Services.Interfaces
{
    public interface IGenericService<T, D> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(params int[] keys);
        Task<T> Add(D dto);
        Task<T> Update(D dto);
        Task<T> Delete(params int[] keys);
    }
}