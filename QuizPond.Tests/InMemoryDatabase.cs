using Microsoft.EntityFrameworkCore;
using QuizPond.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizPond.Tests
{
    public class InMemoryDatabase
    {
        public static ApplicationDbContext GetContextFromInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new ApplicationDbContext(options);

            return context;
        }
    }
}
