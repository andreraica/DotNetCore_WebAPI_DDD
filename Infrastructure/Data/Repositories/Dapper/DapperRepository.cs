using System;
using System.Data;
using System.Data.SqlClient;
using Infrastructure.CrossCutting.Configuration;

namespace Infrastructure.Data.Repositories.Dapper
{
    public class DapperRepository : IDisposable
    {
        protected string connectionString;
        public DapperRepository()
        {
            connectionString =Configuration.ConnectionString;
        }
    
        public IDbConnection Connection
        {
            get  {
                return new SqlConnection(connectionString);
            }
        }
        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
 
    }
}