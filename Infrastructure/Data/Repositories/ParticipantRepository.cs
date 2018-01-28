using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ParticipantRepository : RepositoryBase<Participant> , IParticipantRepository
    {
        public Participant GetByCpf(string cpf)
        {
            return DbSet.FirstOrDefault(user => user.Cpf == cpf &&  user.Active);
        }

        public new IEnumerable<Participant> GetAll()
        {
            return DbSet.AsNoTracking();
        }
    }
}
