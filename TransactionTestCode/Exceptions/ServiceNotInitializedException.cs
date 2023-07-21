namespace TransactionTestCode.Exceptions;

public class ServiceNotInitializedException : Exception
{
    public ServiceNotInitializedException(string serviceName) 
        : base($"Сервис '{serviceName}' не был инициализирован.")
    {
    }
}
