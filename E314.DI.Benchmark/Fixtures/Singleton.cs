namespace E314.DI.Benchmark
{

public interface ISingleton1
{
	void DoSomething();
}

public interface ISingleton2
{
	void DoSomething();
}

public interface ISingleton3
{
	void DoSomething();
}

public class Singleton1 : ISingleton1
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Singleton2 : ISingleton2
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Singleton3 : ISingleton3
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

}