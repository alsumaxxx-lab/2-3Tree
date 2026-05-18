using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp50
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int SIZE = 10000;
            const int SEARCH_COUNT = 100;
            const int DELETE_COUNT = 1000;

            Random rand = new Random();
            int[] numbers = new int[SIZE];
            for (int i = 0; i < SIZE; i++)
                numbers[i] = rand.Next(1, 100000);

            TwoThreeTree tree = new TwoThreeTree();
            List<(double time, int ops)> insertData = new List<(double, int)>();
            List<(double time, int ops)> searchData = new List<(double, int)>();
            List<(double time, int ops)> deleteData = new List<(double, int)>();

            Console.WriteLine("Вставка элементов...");
            for (int i = 0; i < SIZE; i++)
            {
                var sw = Stopwatch.StartNew();
                tree.Insert(numbers[i]);
                sw.Stop();
                insertData.Add((sw.Elapsed.TotalMilliseconds, tree.OperationCount));
            }

            var searchNumbers = numbers.OrderBy(x => rand.Next()).Take(SEARCH_COUNT).ToArray();
            Console.WriteLine("Поиск элементов...");
            foreach (var num in searchNumbers)
            {
                var sw = Stopwatch.StartNew();
                tree.Search(num);
                sw.Stop();
                searchData.Add((sw.Elapsed.TotalMilliseconds, tree.OperationCount));
            }

            var deleteNumbers = numbers.OrderBy(x => rand.Next()).Take(DELETE_COUNT).ToArray();
            Console.WriteLine("Удаление элементов...");
            foreach (var num in deleteNumbers)
            {
                var sw = Stopwatch.StartNew();
                tree.Delete(num);
                sw.Stop();
                deleteData.Add((sw.Elapsed.TotalMilliseconds, tree.OperationCount));
            }

            Console.WriteLine("\n========== РЕЗУЛЬТАТЫ ==========");
            Console.WriteLine($"Вставка (n={SIZE}):");
            Console.WriteLine($"  Среднее время: {insertData.Average(x => x.time):F4} мс");
            Console.WriteLine($"  Среднее кол-во операций: {insertData.Average(x => x.ops):F2}");

            Console.WriteLine($"\nПоиск (n={SEARCH_COUNT}):");
            Console.WriteLine($"  Среднее время: {searchData.Average(x => x.time):F4} мс");
            Console.WriteLine($"  Среднее кол-во операций: {searchData.Average(x => x.ops):F2}");

            Console.WriteLine($"\nУдаление (n={DELETE_COUNT}):");
            Console.WriteLine($"  Среднее время: {deleteData.Average(x => x.time):F4} мс");
            Console.WriteLine($"  Среднее кол-во операций: {deleteData.Average(x => x.ops):F2}");
        }
    }
    
}
