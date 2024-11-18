using EnergyPredictorAPI.Data;
using Microsoft.ML;
using System;
using System.IO;
using System.Linq;

namespace EnergyPredictorAPI.ML
{
    public class ModelTrainer
    {
        private readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "MLModels", "EnergyConsumptionModel.zip");
        private readonly MLContext _mlContext;
        private readonly AppDbContext _dbContext;

        public ModelTrainer(AppDbContext dbContext)
        {
            _mlContext = new MLContext();
            _dbContext = dbContext;
        }

        public void TrainModel()
        {
            // Garante que o diretório para salvar o modelo existe
            var directory = Path.GetDirectoryName(_modelPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Consulta ao banco de dados para obter os dados reais
            var data = _dbContext.ConsumptionData
                .Select(c => new EnergyConsumptionData
                {
                    // Normaliza o timestamp para segundos a partir do Unix Epoch
                    Timestamp = (float)(c.Timestamp - new DateTime(1970, 1, 1)).TotalSeconds,
                    Consumption = (float)c.Consumption
                })
                .ToList();

            if (!data.Any())
            {
                Console.WriteLine("Nenhum dado disponível para treinar o modelo.");
                return;
            }

            var trainingDataView = _mlContext.Data.LoadFromEnumerable(data);

            // Define a pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(EnergyConsumptionData.Timestamp))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Consumption", maximumNumberOfIterations: 100));

            // Treina o modelo
            var model = pipeline.Fit(trainingDataView);

            // Salva o modelo
            _mlContext.Model.Save(model, trainingDataView.Schema, _modelPath);
            Console.WriteLine("Modelo treinado e salvo em: " + _modelPath);
        }

        public float Predict(string deviceType)
        {
            // Verifica se o modelo foi treinado
            if (!File.Exists(_modelPath))
                throw new FileNotFoundException($"Modelo não encontrado em {_modelPath}");

            // Carrega o modelo
            var model = _mlContext.Model.Load(_modelPath, out _);

            // Consulta os dados do banco
            var timestamps = _dbContext.ConsumptionData
                .Where(c => c.DeviceType == deviceType)
                .Select(c => (float)(c.Timestamp - new DateTime(1970, 1, 1)).TotalSeconds)
                .ToList();

            if (!timestamps.Any())
                throw new InvalidOperationException($"Nenhum dado encontrado para o tipo de dispositivo: {deviceType}");

            // Calcula a média do timestamp
            var avgTimestamp = timestamps.Average();

            // Configura o mecanismo de previsão
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EnergyConsumptionData, EnergyConsumptionPrediction>(model);

            // Faz a previsão
            var input = new EnergyConsumptionData { Timestamp = avgTimestamp };
            var prediction = predictionEngine.Predict(input);

            return prediction.PredictedConsumption;
        }
    }
}
