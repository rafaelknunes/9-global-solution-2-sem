using System;
using System.Linq;
using System.Threading.Tasks;
using EnergyPredictorAPI.Data;
using EnergyPredictorAPI.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnergyPredictorAPITests
{
    public class ConsumptionRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetInMemoryDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddConsumptionAsync_ShouldAddConsumptionToDatabase()
        {
            var options = GetInMemoryDbOptions();
            using var context = new AppDbContext(options);
            var repository = new ConsumptionRepository(context);

            var consumptionData = new ConsumptionData
            {
                DeviceId = "TestDevice",
                DeviceType = "TestType",
                Consumption = 50.5M, 
                Timestamp = DateTime.UtcNow
            };

            
            await repository.AddConsumptionAsync(consumptionData);

            var result = context.ConsumptionData.FirstOrDefault(c => c.DeviceId == "TestDevice");
            Assert.NotNull(result);
            Assert.Equal("TestDevice", result.DeviceId);
            Assert.Equal("TestType", result.DeviceType);
            Assert.Equal(50.5M, result.Consumption); 
        }
    }
}
