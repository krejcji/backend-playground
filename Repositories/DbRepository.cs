using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BackendPlayground.Server.Models;

namespace BackendPlayground.Server.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetFirst(Expression<Func<T, bool>> where);
        IEnumerable<T> Get(Expression<Func<T, bool>> where);
    }

    public interface IUserRepository : IRepository<User>
    {
        public IEnumerable<User> GetByName(string name);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            var entry = dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
                dbSet.Add(entity);
            dbContext.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            var entry = dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
            dbContext.SaveChanges();
        }

        public virtual T GetFirst(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).AsEnumerable();
        }

        public virtual void Update(T entity)
        {
            var entry = dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
                dbSet.Attach(entity);
            entry.State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UserDbContext dbContext) : base(dbContext)
        { }

        public IEnumerable<User> GetByName(string name)
        {
            var lowerName = name.ToLower();
            return Get(u => u.UserName.ToLower().Contains(lowerName));
        }
    }
}    
