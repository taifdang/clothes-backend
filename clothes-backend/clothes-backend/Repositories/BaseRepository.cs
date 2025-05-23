﻿using clothes_backend.Data;
using clothes_backend.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace clothes_backend.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext _db;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(DatabaseContext db)
        {
            this._db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual async Task AddBase(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task DeleteBase(T item)
        {
            if(item != null)
            {
               _dbSet.Remove(item);
               await _db.SaveChangesAsync();             
            }
        }

        public async Task<T?> FindBase(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.FirstOrDefaultAsync(condition);
        }

        public virtual async Task<List<T>> GetAllBase()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetIdBase(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IEnumerable<T> PaginationBase(IEnumerable<T> entity, int currentPage, int limit)
        {       
            int skip = (currentPage - 1) * limit;
            entity =  entity.Skip(skip).Take(limit);
            return entity;
        }

        public virtual async Task<T> UpdateBase(int id, T entity)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null) throw new NotImplementedException();
            _db.Entry(item).CurrentValues.SetValues(entity);//same value,type
            await _db.SaveChangesAsync();
            return entity;
        }
        
        
    }
}
