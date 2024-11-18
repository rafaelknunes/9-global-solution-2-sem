using Microsoft.AspNetCore.Mvc;
using EnergyPredictorAPI.Data;
using EnergyPredictorAPI.Models;
using EnergyPredictorAPI.ML;
using System.Threading.Tasks;

namespace EnergyPredictorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyConsumptionController : ControllerBase
    {
        private readonly IConsumptionRepository _repository;
        private readonly ModelTrainer _trainer;

        // Injeção do ModelTrainer no construtor
        public EnergyConsumptionController(IConsumptionRepository repository, ModelTrainer trainer)
        {
            _repository = repository;
            _trainer = trainer;
        }

        /// <summary>
        /// Adiciona um novo registro de consumo de energia.
        /// </summary>
        /// <param name="data">Dados de consumo do dispositivo IoT.</param>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddConsumption([FromBody] ConsumptionData data)
        {
            if (data == null || string.IsNullOrEmpty(data.DeviceId) || data.Consumption <= 0)
                return BadRequest("Dados inválidos.");

            await _repository.AddConsumptionAsync(data);
            return Ok("Dados de consumo adicionados com sucesso.");
        }

        /// <summary>
        /// Obtém todos os registros de consumo de energia.
        /// </summary>
        /// <returns>Lista de registros de consumo.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetConsumptions()
        {
            var data = await _repository.GetConsumptionsAsync();
            return Ok(data);
        }

        /// <summary>
        /// Faz a previsão do consumo de energia com base no tipo de dispositivo.
        /// </summary>
        /// <param name="deviceType">O tipo do dispositivo para o qual a previsão será feita (exemplo: "Geladeira").</param>
        /// <returns>O valor previsto de consumo de energia.</returns>
        [HttpPost("predict")]
        public IActionResult Predict([FromBody] string deviceType)
        {
            // Verifica se o modelo foi treinado
            if (!System.IO.File.Exists(Path.Combine(Environment.CurrentDirectory, "MLModels", "EnergyConsumptionModel.zip")))
            {
                return BadRequest("Modelo não está treinado. Por favor, treine o modelo antes de tentar uma previsão.");
            }

            try
            {
                var predictedConsumption = _trainer.Predict(deviceType);
                return Ok(new { DeviceType = deviceType, PredictedConsumption = predictedConsumption });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar prever o consumo: {ex.Message}");
            }
        }


        /// <summary>
        /// Treina o modelo de previsão de consumo de energia com dados simulados.
        /// </summary>
        /// <returns>Mensagem de sucesso indicando que o modelo foi treinado.</returns>
        [HttpPost("train")]
        public IActionResult TrainModel()
        {
            _trainer.TrainModel();
            return Ok("Modelo treinado com sucesso e salvo no arquivo.");
        }
    }
}
