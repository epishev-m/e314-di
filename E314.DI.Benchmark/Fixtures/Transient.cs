namespace E314.DI.Benchmark
{

public interface ITransient1
{
	void DoSomething();
}

public interface ITransient2
{
	void DoSomething();
}

public interface ITransient3
{
	void DoSomething();
}

public class Transient1 : ITransient1
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Transient2 : ITransient2
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Transient3 : ITransient3
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

}