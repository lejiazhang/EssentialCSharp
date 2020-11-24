using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
			var container = new DependencyContainer();
		
			container.AddTransient<ServiceConsumer>();
			container.AddTransient<HelloService>();
			container.AddSingleton<MessageService>();

			var resolver = new DependencyResolver(container);

			var service1 = resolver.GetService<ServiceConsumer>();
			service1.Print();

			var service2 = resolver.GetService<ServiceConsumer>();
			service2.Print();

			var service3 = resolver.GetService<ServiceConsumer>();
			service3.Print();
		}
    }

	public class DependencyResolver
	{
		private readonly DependencyContainer _dependencyContainer;

		public DependencyResolver(DependencyContainer dependencyContainer)
		{
			_dependencyContainer = dependencyContainer;
		}

		public T GetService<T>() 
		{
			return (T) GetService(typeof(T));
		}

		public object GetService(Type type) 
		{

			var dependency = _dependencyContainer.GetDenpendency(type);
			var constructor = dependency.Type.GetConstructors().Single();
			var parameters = constructor.GetParameters().ToArray();
			
			if (parameters.Length > 0)
			{
				var parameterImplementations = new object[parameters.Length];

				for (int i = 0; i < parameters.Length; i++)
				{
					parameterImplementations[i] = GetService(parameters[i].ParameterType);
				}

				return CreateImplementation(dependency, t => Activator.CreateInstance(t, parameterImplementations));
			}

			return CreateImplementation(dependency, t => Activator.CreateInstance(t));
		}

		public object CreateImplementation(Dependency dependency, Func<Type, object> fatory)
		{
			if (dependency.Implemented)
			{
				return dependency.Implementation;
			}

			var implementation = fatory(dependency.Type);

			if (dependency.DependencyLifetime == DependencyLifetime.Singleton)
			{
				dependency.AddImplementation(implementation);
			}

			return implementation;
		}
	}

	public class DependencyContainer
	{
		List<Dependency> _dependencies;

		public DependencyContainer()
		{
			_dependencies = new List<Dependency>();
		}

		public void AddSingleton<T>()
		{
			_dependencies.Add(new Dependency(typeof(T), DependencyLifetime.Singleton));
		}
		public void AddTransient<T>()
		{
			_dependencies.Add(new Dependency(typeof(T), DependencyLifetime.Transient));
		}

		public Dependency GetDenpendency(Type type)
		{
			return _dependencies.Find(x => x.Type.Name == type.Name);
		}
	}	

	public class Dependency
	{
		public Dependency(Type type, DependencyLifetime dependencyLifetime)
		{
			Type = type;
			DependencyLifetime = dependencyLifetime;
		}

		public Type Type { get; set; }
		public DependencyLifetime  DependencyLifetime { get; set; }

		public object Implementation { get; set; }
		public bool Implemented { get; set; }

		public void AddImplementation(object i)
		{
			Implementation = i;
			Implemented = true;
		}
	}

	public enum DependencyLifetime
	{
		Singleton = 0,
		Transient = 1
	}

	public class ServiceConsumer
	{
		private readonly HelloService _helloService;

		public ServiceConsumer(HelloService helloService)
		{
			_helloService = helloService;
		}

		public void Print()
		{
			_helloService.Print();
		}
	}

	public class HelloService
	{
		private readonly MessageService _messageService;

		public HelloService(MessageService messageService)
		{
			_messageService = messageService;
		}

		public void Print()
		{
			Console.WriteLine($"Hello World {_messageService.Message()}");
		}
	}

	public class MessageService
	{
		int _random;
		public MessageService()
		{
			_random = new Random().Next();
		}

		public string Message() 
		{
			return $" YO #{_random}#";
		}
	}
}
