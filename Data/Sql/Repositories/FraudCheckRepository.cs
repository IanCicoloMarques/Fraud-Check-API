using System.Data;
using System.Diagnostics.CodeAnalysis;
using FraudCheckAPI.Constants.Queries;
using FraudCheckAPI.Data.Sql.Interfaces;
using Dapper;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Models.Responses.Sql;
using Azure.Core;
using FraudCheckAPI.Constants.Enums;
using System.Transactions;
using FraudCheckAPI.Models.DTO;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.ResponseCaching;
using FraudCheckAPI.Models.Responses.External;
using Org.BouncyCastle.Asn1.Ocsp;
using FraudCheckAPI.Models.Responses.Controllers;

namespace FraudCheckAPI.Data.Sql.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FraudCheckRepository : IFraudCheckRepository
    {
        public async Task<Customer> GetCustomer(int customerId, MySqlConnection connection)
        {
            Customer response = new Customer();

            
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = CustomerQueries.SelectQuery;
            command.Parameters.AddWithValue("@customerId", customerId);


            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                response.CustomerId = reader.GetInt32(0);
                response.Name = reader.GetString(1);
                response.Email = reader.GetString(2);
                response.Phone = reader.GetString(3);
                response.Address = reader.GetString(4);
                response.IsActive = reader.GetBoolean(5);
            }



            return response;
        }


        public async Task<List<Customer>> GetCustomerList(MySqlConnection connection)
        {
            List<Customer> response = new List<Customer>();


            MySqlCommand command = connection.CreateCommand();
            command.CommandText = CustomerQueries.ListQuery;
            command.Parameters.AddWithValue("@IsActive", true);


            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Customer customer = new Customer();

                customer.CustomerId = reader.GetInt32(0);
                customer.Name = reader.GetString(1);
                customer.Email = reader.GetString(2);
                customer.Phone = reader.GetString(3);
                customer.Address = reader.GetString(4);
                customer.IsActive = reader.GetBoolean(5);

                response.Add(customer);
            }



            return response;
        }


        public async Task<int> InsertCustomer(CreateCustomerRequest request, IDbConnection connection)
        {
            int insertedId = 0;
            string query = CustomerQueries.InsertQuery;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("Email", request.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("Phone", request.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("Address", request.Address, DbType.String, ParameterDirection.Input);

            try
            {
                insertedId = await connection.QuerySingleAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {

            }

            return insertedId;
        }

        public async Task InsertFraudAlert(FraudCheckResponse response, IDbConnection connection)
        {

            string query = TransactionQueries.InsertFraudAlert;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TransactionId", response.TransactionId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@AlertType", response.Score, DbType.String, ParameterDirection.Input);
            parameters.Add("@AlertDescription", response.Reason, DbType.String, ParameterDirection.Input);

            await connection.QuerySingleAsync(query, parameters);

            return;
        }

        public async Task<long> InsertTransacion(FraudCheckRequest request, IDbConnection connection)
        {

            long insertedId = 0;

            string query = TransactionQueries.StartTransaction;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerId", request.CustomerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Amount", request.Amount, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@Merchant", request.Merchant, DbType.String, ParameterDirection.Input);
            parameters.Add("@Location", request.Location, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestId", request.RequestId, DbType.String, ParameterDirection.Input);



            insertedId = await connection.QuerySingleAsync<long>(query, parameters);

            return insertedId;
        }

        public async Task UpdateTransaction(long transactionId, bool? accepted, DBTransactionStatus status, double score, IDbConnection connection)
        {
            string query = TransactionQueries.UpdateTransaction;

            DynamicParameters parameters = new DynamicParameters();
            
            if(accepted is null)
                parameters.Add("@accepted", null, DbType.Boolean, ParameterDirection.Input);
            else
                parameters.Add("@accepted", !accepted, DbType.Boolean, ParameterDirection.Input);

            parameters.Add("@status", (int)status, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@score", score, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@transacaoId", transactionId, DbType.Int64, ParameterDirection.Input);

            await connection.ExecuteAsync(query, parameters);

            return;
        }

    }
}
