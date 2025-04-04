using System.Linq.Expressions;


namespace NotificationService.Core.RepositoryContracts
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> UpsertAsync(T entity, Expression<Func<T, bool>> predicate);

        Task Delete(T entity);
    }
}