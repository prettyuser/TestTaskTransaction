using TransactionTestCode.Models;

namespace TransactionTestCode.Repositories;

public interface ITransactionRepository
{
    void AddTransaction(int id, DateTime transactionDate, decimal amount);

    Transaction? GetTransactionById(int id);
}
