using FraudCheckAPI.Interfaces;
using FraudCheckAPI.Models.Responses.Sql;
using FraudCheckAPI.Models.Responses.Controllers;
using FraudCheckAPI.Models.Requests.Controllers;
using FraudCheckAPI.Data.Sql.Interfaces;
using System.Data;
using Microsoft.Extensions.Options;
using FraudCheckAPI.Settings;
using MySql.Data.MySqlClient;
using FraudCheckAPI.Constants.Enums;
using FraudCheckAPI.Models.DTO;
using FraudCheckAPI.Models.Responses.External;

namespace FraudCheckAPI.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IFraudCheckRepository _repository;
    private string _connectionString;
     

    public DatabaseService(IFraudCheckRepository repository, IOptions<DatabaseSettings> _settings)
    {
        _repository = repository?? throw new ArgumentNullException(nameof(repository));
        _connectionString = _settings.Value.SQL_FRAUDAPI;
    }

    public async Task<Customer> GetCustomerData(int customerId)
    {
        Customer response = new Customer();

        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        try
        {
            response = await _repository.GetCustomer(customerId, connection);
        }

        catch (Exception ex)
        {

        }


        return response;
    }

    public async Task<List<Customer>> GetCustomerList()
    {
        List<Customer> response = new List<Customer>();

        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        try
        {
            response = await _repository.GetCustomerList(connection);
        }

        catch (Exception ex)
        {

        }


        return response;
    }

    public async Task<CreateCustomerResponse> InsertCustomer(CreateCustomerRequest request)
    {

        CreateCustomerResponse response = new CreateCustomerResponse();

        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        try
        {
            int id = await _repository.InsertCustomer(request, connection);
            if (id > 0)
            {
                response.Success = true;
                response.ErrorCode = "";
                response.CustomerID = id;
            }
            else
            {
                throw new Exception();
            }
        }

        catch (Exception ex)
        {

        }


        return response;
    }

    public async Task InsertFraudAlert(FraudCheckResponse response)
    {
        await using var connection = new MySqlConnection(_connectionString);

        try
        {
            await _repository.InsertFraudAlert(response, connection);
        }
        catch { }
    }

    public async Task<FraudCheckResponse> StartTransaction(FraudCheckRequest request)
    {
        FraudCheckResponse response = new FraudCheckResponse();

        await using var connection = new MySqlConnection(_connectionString);

        try
        {
            long id = await _repository.InsertTransacion(request, connection);
            if (id > 0)
            {
                response.TransactionId = id;
            }
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 1062:
                    response.Accepted = false;
                    response.TransactionId = -1;
                    response.StatusCode = "002";
                    response.Status = "RequestId já utilizado";
                    break;

                case 1452:
                    response.Accepted = false;
                    response.TransactionId = -1;
                    response.StatusCode = "001";
                    response.Status = "CustomerId não encontrado";
                    break;
                default:
                    response.Accepted = false;
                    response.TransactionId = -1;
                    response.StatusCode = "999";
                    response.Status = "Erro interno";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Accepted = false;
            response.TransactionId = -1;
            response.StatusCode = "999";
        }


        return response;
    }

    public async Task UpdateTransaction(FraudCheckResponse response)
    {

        await using var connection = new MySqlConnection(_connectionString);

        try
        {
            if (response.StatusCode.Equals("000"))
                await _repository.UpdateTransaction(response.TransactionId, response.Accepted, DBTransactionStatus.SUCCESS, response.Score, connection);
            else
                await _repository.UpdateTransaction(response.TransactionId, null, DBTransactionStatus.ERROR, response.Score, connection);
        }
        catch (MySqlException ex)
        { }
        catch (Exception ex)
        {

        }
    }
}