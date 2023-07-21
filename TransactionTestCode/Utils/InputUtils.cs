using System.Globalization;

namespace TransactionTestCode.Utils;

public static class InputUtils
{
    public delegate bool TryParseDelegate<T>(string? input, out T result);
    
    public static async Task<T> GetValidInput<T>(string prompt, TryParseDelegate<T> tryParse, Predicate<T> validator)
    {
        T? result = default;
        
        while (result != null && !validator(result))
        {
            Console.WriteLine(prompt);
            if (!tryParse(Console.ReadLine(), out var parsedValue))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректное значение:");
            }
            else
            {
                result = parsedValue;
            }
        }
        
        return result!;
    }
    
    public static async Task<DateTime> GetValidDateTimeInput(string prompt, string dateFormat, Predicate<DateTime> validator)
    {
        DateTime result;

        while (true)
        {
            Console.WriteLine(prompt);
            if (!DateTime.TryParseExact(Console.ReadLine(), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректную дату в формате " + dateFormat + ":");
            }
            else if (!validator(result))
            {
                Console.WriteLine("Некорректная дата. Пожалуйста, введите корректную дату:");
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
