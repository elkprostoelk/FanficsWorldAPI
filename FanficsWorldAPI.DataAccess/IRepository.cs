using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.DataAccess
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        DbSet<TEntity> EntitySet { get; }

        Task<bool> InsertAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);
    }
}
