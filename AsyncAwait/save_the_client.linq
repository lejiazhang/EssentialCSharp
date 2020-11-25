<Query Kind="Program" />

using System.Net.Http;
using System.Threading.Tasks;

HttpClient _client = new HttpClient()
{
	Timeout = TimeSpan.FromSeconds(5)
};
SemaphoreSlim _gate = new SemaphoreSlim(20);

void Main()
{
	Task.WaitAll(CreateCalls().ToArray());
}

public IEnumerable<Task> CreateCalls()
{
	for(int i = 0; i < 500; i++)
	{
		yield return CallBaidu();
	}
}

public async Task CallBaidu()
{
	try
	{
		await _gate.WaitAsync();
		var response = await _client.GetAsync("http://www.baidu.com");
		_gate.Release();
		response.StatusCode.Dump();
	}
	catch(Exception e)
	{
		e.Message.Dump();
	}
}

// You can define other methods, fields, classes and namespaces here
