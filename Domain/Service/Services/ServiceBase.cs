using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Service.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;
        public ServiceBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public virtual TEntity GetById(Guid id)
        {
            return _repositoryBase.GetById(id);            
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _repositoryBase.GetAll();            
        }

        public virtual IEnumerable<TEntity> GetAllActive()
        {
            return _repositoryBase.GetAllActive();
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> expression)
        {
            return _repositoryBase.GetBy(expression);
        }

        public int Total(Expression<Func<TEntity, bool>> expression)
        {
            return _repositoryBase.Total(expression);
        }

        public virtual void Add(TEntity entity)
        {
            _repositoryBase.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _repositoryBase.Update(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            _repositoryBase.AddOrUpdate(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _repositoryBase.Remove(entity);
        }

        public void Commit()
        {
            _repositoryBase?.Commit();
        }

        public void Rollback()
        {
            _repositoryBase?.Rollback();
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }
    }
}
