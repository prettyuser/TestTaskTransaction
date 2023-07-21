using TransactionTestCode.Models;

namespace TransactionTestCode.Repositories;

public class TransactionFactory : ITransactionFactory
{
    public Transaction CreateNewTransaction()
    {
        return new Transaction();
    }
}