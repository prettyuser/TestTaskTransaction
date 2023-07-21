using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TransactionTestCode.Exceptions;
using TransactionTestCode.Repositories;
using TransactionTestCode.Utils;

namespace TransactionTestCode.Services;

public abstract class EntryPoint
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var serviceProvider = ConfigureServiceProvider();
        await using (serviceProvider)
        {
            var program = serviceProvider.GetRequiredService<Program>();
            await program.RunAsync();
        }
    }
    
    private static ServiceProvider ConfigureServiceProvider()
    {
        return new ServiceCollection()
            .AddSingleton<ITransactionFactory, TransactionFactory>()
            .AddSingleton<ITransactionRepository, TransactionRepository>()
            .AddTransient<Program>()
            .BuildServiceProvider();
    }
}

public class Program
{
    private readonly ITransactionRepository _transactionRepository;

    public Program(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("Введите команду (add, get, exit):");
            var command = Console.ReadLine()?.ToLower();

            switch (command)
            {
                case "add":
                    await AddTransactionAsync();
                    break;
                case "get":
                    await GetTransactionAsync();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
    }
    
    private void CheckRepositoryInitialized()
    {
        if (_transactionRepository == null)
        {
            throw new ServiceNotInitializedException(nameof(ITransactionRepository));
        }
    }
    
    private async Task AddTransactionAsync()
    {
        CheckRepositoryInitialized();
        
        var id = await InputUtils.GetValidInput<int>
            ("Введите Id:", int.TryParse, id => id > 0);
        var transactionDate = await InputUtils.GetValidDateTimeInput
            ("Введите дату в формате дд.мм.гггг:", "dd.MM.yyyy", _ => true);
        var amount = await InputUtils.GetValidInput<decimal>
                ("Введите сумму:", decimal.TryParse, amount => amount > 0);

        _transactionRepository.AddTransaction(id, transactionDate, amount);

        Console.WriteLine("[OK]");
    }

    private async Task GetTransactionAsync()
    {
        CheckRepositoryInitialized();
        
        var inputId = await InputUtils.GetValidInput<int>("Введите Id транзакции:", int.TryParse, id => id > 0);
        
        var transaction = _transactionRepository.GetTransactionById(inputId);
        
        if (transaction == null)
        {
            Console.WriteLine("Транзакция с указанным Id не найдена.");
        }
        else
        {
            var transactionJson = JsonConvert.SerializeObject(transaction);
            Console.WriteLine(transactionJson);
            Console.WriteLine("[OK]");
        }
    }
}
