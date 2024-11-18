using Microsoft.ML.Data;

namespace EnergyPredictorAPI.ML
{
    public class EnergyConsumptionPrediction
    {
        [ColumnName("Score")]
        public float PredictedConsumption { get; set; }
    }
}
