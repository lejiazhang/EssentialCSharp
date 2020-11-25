<Query Kind="Program" />

using System.Threading.Tasks;

void Main()
{
	for(int i = 0; i < 10; i ++)
	{
		i.Dump();
		Task.Run(() => SendNotification());
	}
}

// You can define other methods, fields, classes and namespaces here

public void SendNotification()
{
	Task.Delay(1000).GetAwaiter().GetResult();
	"Complete".Dump();
}