namespace FraudCheckAPI.Models.Requests.Controllers
{

    public class FraudCheckRequest
    {
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public string Merchant { get; set; }
        public string Location { get; set; }
        public string RequestId { get; set; }
    }
}

