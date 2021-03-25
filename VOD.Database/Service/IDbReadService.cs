using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VOD.Database.Service
{
    public interface IDbReadService
    {
        Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class;

        Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class;


        Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        void Include<TEntity>() where TEntity : class;

        void Include<TEntity1, TEntity2>() where TEntity1 : class where TEntity2 : class;

        Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class;

        (int courses, int downloads, int instructors, int modules, int videos, int users) Count();

        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class;
    }
}