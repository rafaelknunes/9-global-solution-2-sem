using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EnergyPredictorAPI.Models;

namespace EnergyPredictorAPI.Data
{
    public class ConsumptionRepository : IConsumptionRepository
    {
        private readonly AppDbContext _context;

        public ConsumptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddConsumptionAsync(ConsumptionData data)
        {
            await _context.ConsumptionData.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConsumptionData>> GetConsumptionsAsync()
        {
            return await _context.ConsumptionData.ToListAsync();
        }
    }
}
