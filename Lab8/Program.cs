/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PerformanceTest
{
    public void RunTest()
    {
        var numbers = Enumerable.Range(1, 1000000).ToList();
        var evenNumbers = new List<int>();

        foreach (var number in numbers)
        {
            if (number % 2 == 0)
            {
                evenNumbers.Add(number);
            }
        }

        Console.WriteLine($"Total even numbers (Original): {evenNumbers.Count}");
    }

    public void RunTestLINQ()
    {
        var numbers = Enumerable.Range(1, 1000000).ToList();
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

        Console.WriteLine($"Total even numbers (LINQ): {evenNumbers.Count}");
    }

    public void RunTestPLINQ()
    {
        var numbers = Enumerable.Range(1, 1000000).ToList();
        var evenNumbers = numbers.AsParallel().Where(n => n % 2 == 0).ToList();

        Console.WriteLine($"Total even numbers (PLINQ): {evenNumbers.Count}");
    }

    public void MeasurePerformance(Action testMethod)
    {
        var stopwatch = new Stopwatch();
        long memoryBefore, memoryAfter;

        memoryBefore = GC.GetTotalMemory(true);
        stopwatch.Start();

        testMethod();

        stopwatch.Stop();
        memoryAfter = GC.GetTotalMemory(true);

        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Memory Used: {(memoryAfter - memoryBefore) / (1024 * 1024)} MB");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var test = new PerformanceTest();

        Console.WriteLine("Running Original Test:");
        test.MeasurePerformance(test.RunTest);

        Console.WriteLine("\nRunning LINQ Test:");
        test.MeasurePerformance(test.RunTestLINQ);

        Console.WriteLine("\nRunning PLINQ Test:");
        test.MeasurePerformance(test.RunTestPLINQ);
    }
}
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class NumberProcessor
{
    public void ProcessNumbers(string inputFilePath, string outputFilePath)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();

            // Получение начального объема памяти
            long initialMemory = Process.GetCurrentProcess().WorkingSet64;

            // Чтение чисел из файла, удаление дубликатов и сортировка
            var numbers = File.ReadAllLines(inputFilePath)
                              .Select(int.Parse)
                              .Distinct()
                              .OrderBy(n => n)
                              .ToList();

            // Запись уникальных и отсортированных чисел в выходной файл
            File.WriteAllLines(outputFilePath, numbers.Select(n => n.ToString()));

            stopwatch.Stop();

            // Получение конечного объема памяти
            long finalMemory = Process.GetCurrentProcess().WorkingSet64;
            long memoryUsed = finalMemory - initialMemory;

            Console.WriteLine($"Обработка завершена. Результаты сохранены в '{outputFilePath}'. Время обработки: {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"Используемая оперативная память: {memoryUsed / (1024 * 1024)} МБ");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Ошибка: Файл '{inputFilePath}' не найден.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Файл содержит некорректные данные.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "input.txt"; // Путь к входному файлу
        string outputFilePath = "output.txt"; // Путь к выходному файлу

        // Генерация тестовых файлов
        GenerateTestFile("input_10000.txt", 10000);
        GenerateTestFile("input_1000000.txt", 1000000);

        var processor = new NumberProcessor();

        // Обработка файла с 10,000 чисел
        Console.WriteLine("Обработка файла с 10,000 числами...");
        processor.ProcessNumbers("input_10000.txt", outputFilePath);

        // Обработка файла с 1,000,000 чисел
        Console.WriteLine("Обработка файла с 1,000,000 числами...");
        processor.ProcessNumbers("input_1000000.txt", outputFilePath);
    }

    static void GenerateTestFile(string filePath, int numberOfNumbers)
    {
        Random random = new Random();
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < numberOfNumbers; i++)
            {
                writer.WriteLine(random.Next(1, 100000)); // Генерация случайных чисел
            }
        }
        Console.WriteLine($"Файл '{filePath}' сгенерирован с {numberOfNumbers} числами.");
    }
}