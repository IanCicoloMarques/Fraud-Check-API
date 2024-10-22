namespace FraudCheckAPI.Constants.Queries
{
    public abstract class CustomerQueries
    {
        public const string SelectQuery = @"SELECT
                                                customer_id AS CustomerId
                                                ,name       AS Name
                                                ,email      AS Email
                                                ,phone      AS Phone
                                                ,address    AS Address
                                                ,is_active  AS IsActive
                                            FROM
                                                customers
                                            WHERE
                                                customer_id = @customerId";

        public const string ListQuery = @"SELECT
                                                customer_id AS CustomerId
                                                ,name       AS Name
                                                ,email      AS Email
                                                ,phone      AS Phone
                                                ,address    AS Address
                                                ,is_active  AS IsActive
                                            FROM
                                                customers
                                            WHERE
                                                is_active = @IsActive";

        public const string InsertQuery = @"        INSERT INTO customers
                                                                (name
                                                                ,email
                                                                ,phone
                                                                ,address)
                                                            VALUES
                                                                (@name    
                                                                ,@email  
                                                                ,@phone  
                                                                ,@address);


                                                            SELECT LAST_INSERT_ID();";


        public const string UpdateQuery = @"";
    }

    public abstract class TransactionQueries
    {
        public const string StartTransaction = @"INSERT INTO transactions
                                                    (customer_id
                                                    ,amount
                                                    ,merchant
                                                    ,location
                                                    ,request_id)
                                                VALUES
                                                    (@CustomerId
                                                    ,@Amount
                                                    ,@Merchant
                                                    ,@Location
                                                    ,@RequestId);

                                              SELECT LAST_INSERT_ID();";

        public const string UpdateTransaction = @"UPDATE transactions
                                                    SET
                                                        is_fraud = @accepted,
                                                        transaction_status = @status,
                                                        fraud_score = @score
                                                    WHERE
                                                        transaction_id = @transacaoId";

        public const string InsertFraudAlert = @"INSERT INTO fraud_alerts
                                                    (transaction_id,
                                                    alert_type,
                                                    alert_description)
                                                VALUES
                                                    (@TransactionId,
                                                    @AlertType,
                                                    @AlertDescription)";
    }
}
