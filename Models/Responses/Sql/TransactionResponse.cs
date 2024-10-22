using System;
using System.Transactions;

namespace FraudCheckAPI.Models.Responses.Sql;

public abstract class TransactionResponse
{
    public int CustomerId {  get; set; } 
    public double Amount { get; set; }
    public string Merchant { get; set; }
    public string Location { get; set; }
    public string RequestId { get; set; }
}