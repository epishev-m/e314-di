using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace E314.DI.Benchmark
{

[MemoryDiagnoser]
[RankColumn]
[SuppressMessage("Performance", "CA1822")]
public class Benchmark
{
	[Benchmark]
	public void CreateContainer()
	{
		_ = new DiContainer();
	}

	[Benchmark]
	public void BindTransient()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().To<Transient1>().AsTransient();
	}

	[Benchmark]
	public void BindSingleton()
	{
		var container = new DiContainer();
		container.Bind<ISingleton1>().To<Singleton1>().AsSingle();
	}

	[Benchmark]
	public void BindScope()
	{
		var container = new DiContainer();
		container.Bind<IScope1>().To<Scope1>().AsScoped();
	}

	[Benchmark]
	public void BindComplex()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().To<Transient1>().AsTransient();
		container.Bind<ISingleton1>().To<Singleton1>().AsSingle();
		container.Bind<IScope1>().To<Scope1>().AsScoped();
	}

	[Benchmark]
	public void BindFactory()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().ToFactory(() => new Transient1());
	}

	[Benchmark]
	public void ResolveTransient()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().To<Transient1>().AsTransient();
		container.Resolve<ITransient1>();
	}

	[Benchmark]
	public void ResolveISingleton()
	{
		var container = new DiContainer();
		container.Bind<ISingleton1>().To<Singleton1>().AsSingle();
		container.Resolve<ISingleton1>();
	}

	[Benchmark]
	public void ResolveIScope3()
	{
		var container = new DiContainer();
		container.Bind<IScope1>().To<Scope1>().AsScoped();
		container.Resolve<IScope1>();
	}

	[Benchmark]
	public void ResolveFactory()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().ToFactory(() => new Transient1());
		container.Resolve<ITransient1>();
	}

	[Benchmark]
	public void ResolveComplex()
	{
		var container = new DiContainer();
		container.Bind<ITransient1>().To<Transient1>().AsTransient();
		container.Bind<ISingleton1>().To<Singleton1>().AsSingle();
		container.Bind<IScope1>().To<Scope1>().AsScoped();
		container.Resolve<ITransient1>();
		container.Resolve<ISingleton1>();
		container.Resolve<IScope1>();
	}
}

}