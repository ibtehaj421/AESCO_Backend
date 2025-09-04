using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;

namespace ASCO.Repositories
{
    public class CrewRepository
    {
        private readonly ASCODbContext _context;

        public CrewRepository(ASCODbContext context)
        {
            _context = context;
        }
    }

    //rest of the stuff goes here.
}