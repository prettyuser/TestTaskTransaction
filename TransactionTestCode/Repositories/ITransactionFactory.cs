using TransactionTestCode.Models;

namespace TransactionTestCode.Repositories;

public interface ITransactionFactory
{
    Transaction CreateNewTransaction();
}
