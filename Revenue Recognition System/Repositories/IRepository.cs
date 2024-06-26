namespace Revenue_Recognition_System.Repositories;

public interface IRepository<T> where T : class
{
    Task<ICollection<T?>> GetAll();
    Task<T?> GetById(int id);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(int id);
}