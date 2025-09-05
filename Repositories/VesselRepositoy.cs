using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;


namespace ASCO.Repositories
{
    public class VesselRepository
    {
        private readonly ASCODbContext _context;

        public VesselRepository(ASCODbContext context)
        {
            _context = context;
        }

        // Add methods to interact with vessels, e.g., GetVesselById, AddVessel, UpdateVessel, etc.
        public async Task<int> CreateVesselAsync(Ship vessel)
        {
            await _context.Ships.AddAsync(vessel);
            return await _context.SaveChangesAsync(); //if the value is positive, the vessel was added successfully.
        }
    }
}