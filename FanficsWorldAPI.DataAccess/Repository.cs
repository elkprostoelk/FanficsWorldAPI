using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly FanficsWorldDbContext _dbContext;

        public Repository(FanficsWorldDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> EntitySet => _dbContext.Set<TEntity>();

        public async Task<bool> InsertAsync(TEntity entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(List<TEntity> entities)
        {
            _dbContext.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
