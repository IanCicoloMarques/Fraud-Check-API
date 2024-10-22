using FraudCheckAPI.Interfaces;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Requests.External;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Responses.External;
using FraudCheckAPI.Settings;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace FraudCheckAPI.Services
{
    public class AnalysisService : IAnalysisService
    {
        private AnalysisApiSettings _settings;
        private RestClientOptions _options;
        public AnalysisService(IOptions<AnalysisApiSettings> settings)
        {
            _settings = settings.Value;
            _options = new RestClientOptions(_settings.Base);
        }


        public async Task<FraudAnalysisResponse> RequestAnalysis(FraudCheckRequest request)
        {
            FraudAnalysisResponse result = new();

            FraudAnalysisRequest analysisRequest = new();

            analysisRequest.amount = request.Amount;
            analysisRequest.location = request.Location;

            RestClient client = new RestClient(_options);
            RestRequest httpRequest = new RestRequest(_settings.CreditAnalysisEndpoint, Method.Post);
            httpRequest.AddBody(request);

            var response = await client.ExecuteAsync(httpRequest);

            if (response.IsSuccessStatusCode){
                result = JsonSerializer.Deserialize<FraudAnalysisResponse>(response.Content);
                result.IsSuccessful = true;
            }
            else
            {
                result.status = "Erro no módulo de análise";
                result.statusCode = "999";
                result.IsSuccessful = false;
            }
            return result;
        }
    }
}
