using System;
using System.Linq;
using E314.DataTypes;
using E314.Protect;

namespace E314.DI
{

/// <summary>
/// Represents a concrete implementation of <see cref="IDiBinding"/> for dependency injection.
/// Manages the association between a key type and its instance providers, and provides configuration methods.
/// </summary>
public sealed class DiBinding : Binding<Type, IInstanceProvider>, IDiBinding
{
	private readonly ITypeAnalyzer _typeAnalyzer;
	private readonly IDiContainer _container;

	/// <summary>
	/// Initializes a new instance of the <see cref="DiBinding"/> class.
	/// </summary>
	/// <param name="capacityStrategy">The strategy for managing capacity.</param>
	/// <param name="typeAnalyzer">The type analyzer used for inspecting types.</param>
	/// <param name="container">The dependency injection container.</param>
	/// <param name="key">The key type for this binding.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="typeAnalyzer"/> or <paramref name="container"/> is null.</exception>
	public DiBinding(ICapacityStrategy capacityStrategy, ITypeAnalyzer typeAnalyzer, IDiContainer container, Type key)
		: base(capacityStrategy, key)
	{
		Requires.NotNull(typeAnalyzer, nameof(typeAnalyzer));
		Requires.NotNull(container, nameof(container));
		_typeAnalyzer = typeAnalyzer;
		_container = container;
	}

	/// <summary>
	/// Indicates whether the binding is configured to provide scoped instances.
	/// </summary>
	public bool IsScopeInstance => Values[0] is ScopeInstanceProvider;

	/// <summary>
	/// Configures the binding to resolve instances of the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to resolve.</typeparam>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding To<T>()
	{
		Requires.NotDisposed(IsDisposed);
		var instanceProvider = new ActivatorInstanceProvider(typeof(T), _typeAnalyzer, _container);
		return (IDiBinding)base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to resolve the specified instance.
	/// </summary>
	/// <param name="instance">The instance to resolve.</param>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding To(object instance)
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotNull(instance, nameof(instance));
		var instanceProvider = new InstanceProvider(instance);
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to resolve instances of the key type itself.
	/// </summary>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding ToSelf()
	{
		Requires.NotDisposed(IsDisposed);
		var instanceProvider = new ActivatorInstanceProvider(Key, _typeAnalyzer, _container);
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to resolve instances using a factory of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The factory type, which must implement <see cref="IFactory"/>.</typeparam>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding ToFactory<T>() where T : IFactory
	{
		Requires.NotDisposed(IsDisposed);
		var instanceProvider = new FactoryInstanceProvider(
			new ActivatorInstanceProvider(typeof(T), _typeAnalyzer, _container));
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to resolve instances using the specified factory.
	/// </summary>
	/// <param name="factory">The factory to use for instance creation.</param>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding ToFactory(IFactory factory)
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotNull(factory, nameof(factory));
		var instanceProvider = new FactoryInstanceProvider(
			new InstanceProvider(factory));
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to resolve instances using the specified factory function.
	/// </summary>
	/// <param name="factory">The factory function that creates instances of the binding type.</param>
	/// <returns>The current binding instance for chaining.</returns>
	public IDiBinding ToFactory(Func<object> factory)
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotNull(factory, nameof(factory));
		var instanceProvider = new FactoryInstanceProvider(new Factory(factory));
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to use a custom instance provider
	/// </summary>
	/// <remarks>
	/// This method is primarily intended for custom functionality extensions,
	/// allowing custom instance creation logic through <see cref="IInstanceProvider"/> implementation
	/// </remarks>
	/// <param name="instanceProvider">Custom instance provider implementing <see cref="IInstanceProvider"/></param>
	/// <returns>Current binding instance for chaining</returns>
	public IDiBinding ToInstanceProvider(IInstanceProvider instanceProvider)
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotNull(instanceProvider, nameof(instanceProvider));
		return (IDiBinding) base.To(instanceProvider);
	}

	/// <summary>
	/// Configures the binding to provide a single shared instance (singleton) for all resolutions.
	/// </summary>
	public void AsSingle()
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotEmpty(Values, nameof(Values));
		var values = Values.ToArray();
		ClearValues();

		foreach (var provider in values)
		{
			var instanceProvider = new SingletonInstanceProvider(provider);
			base.To(instanceProvider);
		}
	}

	/// <summary>
	/// Configures the binding to provide a new instance per scope.
	/// </summary>
	public void AsScoped()
	{
		Requires.NotDisposed(IsDisposed);
		Requires.NotEmpty(Values, nameof(Values));
		var values = Values.ToArray();
		ClearValues();

		foreach (var provider in values)
		{
			var instanceProvider = new ScopeInstanceProvider(provider);
			base.To(instanceProvider);
		}
	}

	/// <summary>
	/// Configures the binding to provide a new instance for every resolution (transient).
	/// </summary>
	public void AsTransient()
	{
		Requires.NotDisposed(IsDisposed);
	}
}

}