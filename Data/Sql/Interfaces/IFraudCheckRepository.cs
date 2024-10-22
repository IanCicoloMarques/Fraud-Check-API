using FraudCheckAPI.Constants.Enums;
using FraudCheckAPI.Models.DTO;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Responses.External;
using FraudCheckAPI.Models.Responses.Sql;
using MySql.Data.MySqlClient;
using System.Data;

namespace FraudCheckAPI.Data.Sql.Interfaces
{
    public interface IFraudCheckRepository
    {
        public Task<Customer> GetCustomer(int customerId, MySqlConnection connection);
        public Task<List<Customer>> GetCustomerList(MySqlConnection connection);
        public Task<int> InsertCustomer(CreateCustomerRequest request, IDbConnection connection);
        public Task InsertFraudAlert(FraudCheckResponse response, IDbConnection connection);
        public Task<long> InsertTransacion(FraudCheckRequest request, IDbConnection connection);
        public Task UpdateTransaction(long transactionId, bool? accepted, DBTransactionStatus SUCCESS, double score, IDbConnection connection);
    }
}