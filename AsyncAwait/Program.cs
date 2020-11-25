using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new System.Net.Http.HttpClient();
            var task = client.GetStringAsync("http://www.baidu.com");

            var a = 0;
            for (int i = 0; i < 1_000_000; i++)
            {
                a += i;
            }
            
            var page = await task;
            Console.WriteLine("Hello World!");
        }
    }
}
