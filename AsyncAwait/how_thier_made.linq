<Query Kind="Program" />

using System.Threading.Tasks;
using System.Collections.Concurrent;

void Main()
{
	var channel = new Channel<string>();
	
	Task.WaitAll(Consumer(channel), Producer(channel), Producer(channel), Producer(channel));
}

public async Task Producer(IWrite<string> writer)
{
	for(int i = 0; i < 100; i ++)
	{
		writer.Push(i.ToString());
		await Task.Delay(100);
	}
	
	writer.Complete();
}

public async Task Consumer(IRead<string> reader)
{
	while(!reader.IsCompleted())
	{
		var msg = await reader.Read();
		
		msg.Dump("msg");
	}
}


public interface IRead<T>
{
	Task<T> Read();
	bool IsCompleted();
}

public interface IWrite<T>
{
	void Push(T msg); 
	void Complete();
}


public class Channel<T>: IRead<T>, IWrite<T>
{
	private bool Finished;
	private ConcurrentQueue<T> _queue;
	private SemaphoreSlim _flag;
	
	public Channel()
	{
		_queue = new ConcurrentQueue<T>();
		_flag = new SemaphoreSlim(0);
	}
	
	public void Push(T msg)
	{
		_queue.Enqueue(msg);
		_flag.Release();
	}
	
	public async Task<T> Read()
	{
		await _flag.WaitAsync();
		
		if(_queue.TryDequeue(out var msg))
		{
			return msg;
		}
		
		return default;
	}
	
	public void Complete()
	{
		Finished = true;
	}
	
	public bool IsCompleted()
	{
		return Finished && _queue.IsEmpty;
	}
}

// You can define other methods, fields, classes and namespaces here
