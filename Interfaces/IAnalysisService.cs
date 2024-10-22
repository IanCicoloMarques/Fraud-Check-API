using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Requests.External;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Responses.External;

namespace FraudCheckAPI.Interfaces
{
    public interface IAnalysisService
    {
        public Task<FraudAnalysisResponse> RequestAnalysis(FraudCheckRequest request);
    }
}
