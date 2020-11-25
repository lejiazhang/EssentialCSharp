<Query Kind="Program" />

using System.Threading.Tasks;

async Task Main()
{
	Thread.CurrentThread.ManagedThreadId.Dump("1");
	var client = new System.Net.Http.HttpClient();
	Thread.CurrentThread.ManagedThreadId.Dump("2");
	var task = client.GetStringAsync("http://www.baidu.com");
	Thread.CurrentThread.ManagedThreadId.Dump("3");
	var a = 0;
	for(int i = 0; i < 1_000_000; i ++)
	{
		a +=i;
	}
	Thread.CurrentThread.ManagedThreadId.Dump("4");
	var page = await task;
	Thread.CurrentThread.ManagedThreadId.Dump("5");
}