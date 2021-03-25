using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VOD.Database.Context;
using VOD.Database.Service;

namespace VOD.API.Services
{
    public class CrudService : ICrudService
    {
        private readonly VodContext _db;
        private readonly IDbReadService _dbRead;
        private readonly IMapper _mapper;

        public CrudService(IDbReadService dbRead, VodContext db, IMapper mapper)
        {
            _dbRead = dbRead;
            _db = db;
            _mapper = mapper;
        }


        public async Task<List<TDto>> GetAsync<TEntity, TDto>(bool include = false)
            where TEntity : class where TDto : class
        {
            if (include) _dbRead.Include<TEntity>();
            var entities = await _dbRead.GetAsync<TEntity>();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> GetSingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression,
            bool include = false) where TEntity : class where TDto : class
        {
            if (include) _dbRead.Include<TEntity>();
            var entity = await _dbRead.GetSingleAsync(expression);

            return _mapper.Map<TDto>(entity);
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class
        {
            return await _db.Set<TEntity>().AnyAsync(expression);
        }

        public async Task<int> CreateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class
        {
            try
            {
                var entity = _mapper.Map<TEntity>(item);
                _db.Add((object) entity);
                var succeed = await _db.SaveChangesAsync() >= 0;
                if (succeed) return (int) entity.GetType().GetProperty("Id").GetValue(entity); //reflection
            }
            catch
            {
            }

            return -1;
        }

        public async Task<bool> UpdateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class
        {
            try
            {
                var entity = _mapper.Map<TEntity>(item);
                _db.Update((object) entity);
                return await _db.SaveChangesAsync() >= 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class
        {
            try
            {
                var entity = await _dbRead.GetSingleAsync(expression);
                _db.Remove((object) entity);
                return await _db.SaveChangesAsync() >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}