using Infrastructure.CrossCutting.Configuration;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AADbContext>
    {
        public AADbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AADbContext>();
            
            builder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=CoreApiDDD;Integrated Security=True;MultipleActiveResultSets=True");
            return new AADbContext(builder.Options);
        }
    }
}
