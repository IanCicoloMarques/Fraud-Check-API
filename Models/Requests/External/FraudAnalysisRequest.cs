namespace FraudCheckAPI.Models.Requests.External
{
    public class FraudAnalysisRequest
    {
        public double amount { get; set; }
        public string location { get; set; }
    }
}
