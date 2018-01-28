using Domain.Interfaces.UnitOfWork;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Dapper
{
    public interface IParticipantDapper
    {
        Participant GetById(Guid id);
        Participant GetByCpf(string cpf);
        IEnumerable<Participant> GetAll();
        IEnumerable<Participant> GetAllActive();
        IEnumerable<Participant> GetBy(Expression<Func<Participant, bool>> expression);
        int Total(Expression<Func<Participant, bool>> expression);
        void Add(Participant entity);
        void Update(Participant entity);
        void AddOrUpdate(Participant entity);
        void Remove(Participant entity);
        void Dispose();
    }
}
