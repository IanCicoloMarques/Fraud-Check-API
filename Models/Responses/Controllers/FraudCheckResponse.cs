namespace FraudCheckAPI.Models.Responses.Controllers
{
    public class FraudCheckResponse
    {
        public string Status { get; set; }
        public bool Accepted { get; set; }
        public string Reason { get; set; }
        public string StatusCode { get; set; }
        public long TransactionId { get; set; }
        public double Score { get; set; }
    }
}
