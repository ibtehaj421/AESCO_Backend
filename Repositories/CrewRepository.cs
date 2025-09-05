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

        //updations
        public async Task<int> UpdateCrewAsync(User request)
        {

            _context.Users.Update(request);
            return await _context.SaveChangesAsync();
        }

        //assign crew to vessel
        public async Task<int> AssignCrewToVesselAsync(ShipAssignment assignment)
        {
            await _context.ShipAssignments.AddAsync(assignment);
            return await _context.SaveChangesAsync(); //if the value is positive, the assignment was added successfully.
        }

        //get active assignment by assignment id
        public async Task<ShipAssignment?> GetActiveAssignmentById(int id)
        {
            return await _context.ShipAssignments
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        //unassign crew from vessel (for now im keeping to update the unassignment date)
        public async Task<int> UnassignCrewFromVesselAsync(ShipAssignment assignment)
        {
            //update only required fields
            _context.ShipAssignments.Attach(assignment);
            _context.Entry(assignment).Property(a => a.UnassignedAt).IsModified = true;
            _context.Entry(assignment).Property(a => a.Status).IsModified = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAssignmentDetailsAsync(ShipAssignment assignment)
        {
            _context.ShipAssignments.Update(assignment);
            return await _context.SaveChangesAsync();
        }
    }

    //rest of the stuff goes here.

    
   
    
}