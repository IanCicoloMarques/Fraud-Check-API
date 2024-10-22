using FraudCheckAPI.Interfaces;
using FraudCheckAPI.Models.DTO;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Responses.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FraudCheckAPI.Controllers
{
    [ApiController]
    [Route("management")]
    public class Management : ControllerBase
    {
        private readonly ILogger<FraudCheckApi> _logger;
        private readonly IDatabaseService _databaseService;

        public Management(ILogger<FraudCheckApi> logger, IDatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpPost]
        [Route("customer")]
        public async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request)
        {

            CreateCustomerResponse response = new CreateCustomerResponse();

            try
            {
                response = await _databaseService.InsertCustomer(request);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.CustomerID = 0;
                response.ErrorCode = ex.Message;
            }
            finally
            {

            }

            return response;
        }

        [HttpGet]
        [Route("customer/{customerId}")]
        public async Task<Customer> GetCustomerData(int customerId)
        {

            Customer response = new Customer();

            try
            {
                response = await _databaseService.GetCustomerData(customerId);
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

            return response;
        }


        [HttpGet]
        [Route("customer/list")]
        public async Task<List<Customer>> GetCustomerData()
        {

            List<Customer> response = new List<Customer>();

            try
            {
                response = await _databaseService.GetCustomerList();
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }

            return response;
        }
    }
}
