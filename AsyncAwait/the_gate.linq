<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <RuntimeVersion>3.1</RuntimeVersion>
</Query>

using System.Threading.Tasks;

SemaphoreSlim gate = new SemaphoreSlim(1);

async Task Main()
{
	for(int i = 0; 1 < 10; i ++)
	{
		"Start".Dump();
		 await gate.WaitAsync();
		"Do Some Work".Dump();
		gate.Release();
		"Finish".Dump();
	}
}

// You can define other methods, fields, classes and namespaces here
