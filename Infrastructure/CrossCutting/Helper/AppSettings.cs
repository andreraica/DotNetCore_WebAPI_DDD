﻿using System;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.CrossCutting.Helper
{
    public static class AppSettings
    {
        public static IConfiguration Configuration { get; set; }

        public static T Get<T>(string key)
        {
            if (Configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                Configuration = configuration.GetSection("ApiGrayLog");
            }

            return (T)Convert.ChangeType(Configuration[key], typeof(T));
        }
    }
}
