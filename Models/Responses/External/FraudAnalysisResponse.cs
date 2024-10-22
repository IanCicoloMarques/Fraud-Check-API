namespace FraudCheckAPI.Models.Responses.External
{
    public class FraudAnalysisResponse
    {
        public string status { get; set; }
        public string statusCode { get; set; }
        public string reason { get; set; }
        public bool accepted { get; set; }
        public double score { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
