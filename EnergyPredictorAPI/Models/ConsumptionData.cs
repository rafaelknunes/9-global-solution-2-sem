using System;

namespace EnergyPredictorAPI.Models
{
    public class ConsumptionData
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
        public string DeviceType { get; set; } 
    }
}
