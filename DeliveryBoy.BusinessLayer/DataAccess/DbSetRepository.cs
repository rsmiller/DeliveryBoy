using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DeliveryBoy.BusinessLayer.DataAccess
{
    public class DbSetRepository<TEntity>
            where TEntity : class
    {
        public DbSetRepository(ITipsContext context, DbSet<TEntity> dbSet)
        {
            this.DbSet = dbSet;
            this._Context = context;
        }

        protected DbSet<TEntity> DbSet { get; set; }
        private ITipsContext _Context;

        public IQueryable<TEntity> All { get { return this.DbSet.AsQueryable(); } }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression,
                                        params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return this.All.Where(expression);
            }

            return includes.Aggregate(this.All.Where(expression), (current, include) => current.Include(include));
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression,
                                      params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return this.All.FirstOrDefault(expression);
            }

            return
                includes.Aggregate(this.All.Where(expression), (current, include) => current.Include(include))
                        .FirstOrDefault();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression,
                                       params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return this.All.SingleOrDefault(expression);
            }

            return
                includes.Aggregate(this.All.Where(expression), (current, include) => current.Include(include))
                        .SingleOrDefault();
        }

        public EntityEntry<TEntity> Add(TEntity item)
        {
            return this.DbSet.Add(item);
        }

        public EntityEntry<TEntity> Remove(TEntity item)
        {
            return this.DbSet.Remove(item);
        }

        public EntityEntry<TEntity> Update(TEntity item)
        {
            return this.DbSet.Update(item);
        }

        public int SaveChanges()
        {
            return this._Context.SaveChanges();
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.DbSet.Count(expression);
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return this.DbSet.Any(expression);
        }
    }
}
