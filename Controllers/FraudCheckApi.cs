using FraudCheckAPI.Interfaces;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Responses.External;
using FraudCheckAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FraudCheckAPI.Controllers
{
    [ApiController]
    [Route("fraud")]
    public class FraudCheckApi : ControllerBase
    {


        private readonly ILogger<FraudCheckApi> _logger;
        private readonly IDatabaseService _databaseService;
        private readonly IAnalysisService _externalService;

        public FraudCheckApi(ILogger<FraudCheckApi> logger, IDatabaseService databaseService, IAnalysisService externalService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _externalService = externalService;
        }


        [HttpPost]
        [Route("check-transaction")]
        public async Task<FraudCheckResponse> PostFraudCheck(FraudCheckRequest request)
        {

            FraudCheckResponse response = new FraudCheckResponse();

            try
            {
                response = await _databaseService.StartTransaction(request);

                if (response is not null
                    && response.TransactionId > 0)
                {
                    FraudAnalysisResponse analysisResponse = await _externalService.RequestAnalysis(request);

                    if (analysisResponse.IsSuccessful)
                    {
                        response.Accepted = analysisResponse.accepted;
                        response.StatusCode = "000";
                        response.Status = analysisResponse.status;
                        response.Score = analysisResponse.score;
                        response.Reason = analysisResponse.reason;

                        if (!analysisResponse.accepted)
                        {
                            await _databaseService.InsertFraudAlert(response);
                        }
                    }
                    else
                    {
                        response.Status = analysisResponse.status;
                        response.StatusCode = analysisResponse.statusCode;
                    }

                    await _databaseService.UpdateTransaction(response);


                }
            }
            catch (Exception ex)
            {
                response.Accepted = false;
                response.TransactionId = 0;
                response.StatusCode = ex.Message;

            }
            finally
            {

            }

            return response;
        }
    }
}
