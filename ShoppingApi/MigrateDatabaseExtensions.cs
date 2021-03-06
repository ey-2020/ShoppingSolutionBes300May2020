﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi
{
    public static class MigrateDatabaseExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using(var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                } catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Could Not Migrate the Database");
                    // throw ex;
                    // Julie Lehrman (the goddess of SQL Server and Entity Framework, didn't have this in her code.
                    // I added it because I temporarily thought I was smarter than her. Don't EVER do that. Nobody is better.
                }

                return webHost;
            }
        }
    }
}
