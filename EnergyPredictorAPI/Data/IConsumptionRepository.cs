using System.Collections.Generic;
using System.Threading.Tasks;
using EnergyPredictorAPI.Models;

namespace EnergyPredictorAPI.Data
{
    public interface IConsumptionRepository
    {
        Task AddConsumptionAsync(ConsumptionData data);
        Task<IEnumerable<ConsumptionData>> GetConsumptionsAsync();
    }
}
