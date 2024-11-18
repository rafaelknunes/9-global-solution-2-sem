using EnergyPredictorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnergyPredictorAPI.Data
{
    public static class SeedData
    {
        public static void SeedDatabase(AppDbContext context)
        {
            // Verifica explicitamente se há registros
            bool hasData = context.ConsumptionData.Take(1).Count() > 0;

            if (!hasData)
            {
                var deviceTypes = new[] { "TV", "Geladeira", "ArCondicionado", "Microondas", "Máquina de Lavar" };
                var random = new Random();
                var simulatedData = new List<ConsumptionData>();

                foreach (var deviceType in deviceTypes)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        simulatedData.Add(new ConsumptionData
                        {
                            DeviceId = Guid.NewGuid().ToString(),
                            Timestamp = DateTime.Now.AddMinutes(-i * 10),
                            Consumption = random.Next(50, 300),
                            DeviceType = deviceType
                        });
                    }
                }

                context.ConsumptionData.AddRange(simulatedData);
                context.SaveChanges();

                Console.WriteLine("Dados simulados inseridos com sucesso!");
            }
            else
            {
                Console.WriteLine("O banco já contém dados.");
            }
        }


    }
}
