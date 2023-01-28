﻿using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories
{
    public interface IRepository<TEntity> where TEntity:class
    {
        public Task<List<TEntity>> GetByFuncExpAsync(Func<TEntity, bool>? func=null);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func);
        public Task<TEntity> CreateAsync(TEntity model);
        public Task<bool> DeleteAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity model);
        public Task SaveChangesAsync();
    }
}
