using System;
using System.Linq;
using Tourplanner.Entities;

namespace Tourplanner
{
    public static class DbInitializer
    {
        public static void Initialize(TourContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    } 
}