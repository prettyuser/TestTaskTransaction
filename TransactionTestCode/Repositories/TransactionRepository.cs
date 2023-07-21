using System.Collections.Concurrent;
using TransactionTestCode.Models;

namespace TransactionTestCode.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private static readonly ConcurrentDictionary<int, Transaction> Transactions = new ();
    private static readonly ReaderWriterLockSlim RwLock = new ();
    private readonly ITransactionFactory _transactionFactory;

    public TransactionRepository(ITransactionFactory transactionFactory)
    {
        _transactionFactory = transactionFactory;
    }
    
    public void AddTransaction(int id, DateTime transactionDate, decimal amount)
    {
        var transaction = _transactionFactory.CreateNewTransaction();

        transaction.Id = id;
        transaction.TransactionDate = transactionDate;
        transaction.Amount = amount;
        
        try
        {
            RwLock.EnterWriteLock();
            Transactions[id] = transaction;
        }
        finally
        {
            RwLock.ExitWriteLock();
        }
    }

    public Transaction? GetTransactionById(int id)
    {
        Transaction? transaction;
        try
        {
            RwLock.EnterReadLock();
            Transactions.TryGetValue(id, out transaction);
        }
        finally
        {
            RwLock.ExitReadLock();
        }
        return transaction;
    }
}
