namespace FraudCheckAPI.Models.Responses.Controllers
{
    public class CreateCustomerResponse
    {
        public int CustomerID { get; set; }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }

    }
}
