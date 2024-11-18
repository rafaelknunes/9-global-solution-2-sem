using Microsoft.ML.Data;

namespace EnergyPredictorAPI.ML
{
    public class EnergyConsumptionData
    {
        [LoadColumn(0)]
        public string DeviceType { get; set; } 

        [LoadColumn(1)]
        public float Consumption { get; set; }

        [LoadColumn(2)]
        public float Timestamp { get; set; }
    }
}
