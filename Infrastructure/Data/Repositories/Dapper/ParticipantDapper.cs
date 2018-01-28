using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repositories.Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories.Dapper
{
    public class ParticipantDapper : DapperRepository, IDisposable, IParticipantDapper
    {

        public virtual Participant GetById(Guid id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT Id"
                                 + " ,Active"
                                 + " ,BirthDate"
                                 + " ,Cpf"
                                 + " ,DateModified"
                                 + " ,Name"
                                 + " ,Gender"
                                 + " FROM [Participant]"
                            + " WHERE Id = @Id";
                dbConnection.Open();
                return dbConnection.Query<Participant>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public Participant GetByCpf(string cpf)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT Id"
                                 + " ,Active"
                                 + " ,BirthDate"
                                 + " ,Cpf"
                                 + " ,DateModified"
                                 + " ,Name"
                                 + " ,Gender"
                                 + " FROM [Participant]"
                            + " WHERE Cpf = @Cpf";
                dbConnection.Open();
                return dbConnection.Query<Participant>(sQuery,new {cpf = cpf}).FirstOrDefault();
            }
        }

        public IEnumerable<Participant> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                return dbConnection.Query<Participant>("SELECT Id"
                                         + " ,Active"
                                         + " ,BirthDate"
                                         + " ,Cpf"
                                         + " ,DateModified"
                                         + " ,Name"
                                         + " ,Gender"
                                         + " FROM [Participant]");
            }
        }

        public IEnumerable<Participant> GetAllActive()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Participant>("SELECT Id"
                             + " ,Active"
                             + " ,BirthDate"
                             + " ,Cpf"
                             + " ,DateModified"
                             + " ,Name"
                             + " ,Gender"
                             + " FROM [Participant] WHERE Active=1");
            }
        }

        public IEnumerable<Participant> GetBy(Expression<Func<Participant, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public int Total(Expression<Func<Participant, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Add(Participant obj)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO [Participant] (Id,Active,BirthDate, Cpf, Name, Gender, DateModified)"
                                + " VALUES(@Id, @Active, @BirthDate, @Cpf, Name, @Gender, @DateModified)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, obj);
            }
        }

        public void Update(Participant obj)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE [Participant] SET Address	= @Address,"                            
                            + " BirthDate = @BirthDate, "
                            + " Cpf = @Cpf, "
                            + " Name = @Name, "
                            + " Gender = @Gender, "
                            + " DateModified = @DateModified, "
                            + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, obj);
            }
        }

        public void AddOrUpdate(Participant entity)
        {
            if (entity == null) return;

            if (entity.Id.GetHashCode() == 0){
                entity.Id = Guid.NewGuid();
                Add(entity);
            }                
            else
                Update(entity);
        }

        public virtual void Remove(Participant obj)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var id = obj.Id;

                string sQuery = "DELETE FROM [Participant]"
                            + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        
        public void Commit()
        {
            try
            {

            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Rollback()
        {
            try
            {

            }
            catch
            {
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
