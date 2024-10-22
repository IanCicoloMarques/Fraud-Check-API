using System.Data;
using System.Threading.Tasks;
using FraudCheckAPI.Models.DTO;
using FraudCheckAPI.Models.Requests;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Responses;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Responses.External;

namespace FraudCheckAPI.Interfaces;

public interface IDatabaseService
{
    public Task<Customer> GetCustomerData(int customerId);
    public Task<List<Customer>> GetCustomerList();
    public Task<CreateCustomerResponse> InsertCustomer(CreateCustomerRequest request);
    public Task InsertFraudAlert(FraudCheckResponse response);
    public Task<FraudCheckResponse> StartTransaction(FraudCheckRequest request);
    public Task UpdateTransaction(FraudCheckResponse response);
}