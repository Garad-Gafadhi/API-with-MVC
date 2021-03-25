using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VOD.Database.Context;

namespace VOD.Database.Service
{
    public class DbReadService : IDbReadService
    {
        private readonly VodContext _db;

        public DbReadService(VodContext db)
        {
            _db = db;
        }

        public async Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            return await _db.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            return await _db.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _db.Set<TEntity>().AnyAsync(expression);
        }

        public void Include<TEntity>() where TEntity : class
        {
            var propertyNames =
                _db.Model.FindEntityType(typeof(TEntity)).GetNavigations().Select(e => e.Name);
            foreach (var name in propertyNames) _db.Set<TEntity>().Include(name).Load();
        }

        public void Include<TEntity1, TEntity2>() where TEntity1 : class where TEntity2 : class
        {
            Include<TEntity1>();
            Include<TEntity2>();
        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            return await _db.Set<TEntity>().SingleOrDefaultAsync(expression);
        }

        public (int courses, int downloads, int instructors, int modules, int videos, int users) Count()
        {
            return (
                courses: _db.Courses.Count(),
                downloads: _db.Downloads.Count(),
                instructors: _db.Instructors.Count(),
                modules: _db.Modules.Count(),
                videos: _db.Videos.Count(),
                users: _db.Users.Count());
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class
        {
            return await _db.Set<TEntity>().AnyAsync(expression);
        }
    }
}