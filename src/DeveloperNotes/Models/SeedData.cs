using DeveloperNotes.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            if (context.Database == null)
            {
                throw new Exception("DB is null");
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    
                );
            }

            context.SaveChanges();
        }
    }
}