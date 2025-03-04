namespace E314.DI.Benchmark
{

public interface IScope1
{
	void DoSomething();
}

public interface IScope2
{
	void DoSomething();
}

public interface IScope3
{
	void DoSomething();
}

public class Scope1 : IScope1
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Scope2 : IScope2
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

public class Scope3 : IScope3
{
	public void DoSomething()
	{
		Console.WriteLine("Hello");
	}
}

}