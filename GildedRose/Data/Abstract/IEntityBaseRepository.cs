using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GildedRose.Entities;

namespace GildedRose.Data.Abstract
{
    public interface IEntityBaseRepository<T> where T : IEntityBase, new()
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Commit();
    }
}