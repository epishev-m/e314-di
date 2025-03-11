using System;
using System.Collections.Generic;
using E314.DataTypes;

namespace E314.DI
{

/// <summary>
/// Represents a binding for dependency injection, associating a key type with one or more instance providers.
/// Provides methods to configure how instances are created and managed (e.g., singleton, scoped, transient).
/// </summary>
public interface IDiBinding
{
	/// <summary>
	/// Gets the key type associated with this binding.
	/// </summary>
	Type Key { get;	}

	/// <summary>
	/// Gets the list of instance providers associated with this binding.
	/// </summary>
	IReadOnlyList<IInstanceProvider> Values{ get; }

	/// <summary>
	/// Indicates whether the binding is configured to provide scoped instances.
	/// </summary>
	bool IsScopeInstance { get; }

	/// <summary>
	/// Configures the binding to resolve instances of the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to resolve.</typeparam>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding To<T>();

	/// <summary>
	/// Configures the binding to resolve the specified instance.
	/// </summary>
	/// <param name="instance">The instance to resolve.</param>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding To(object instance);

	/// <summary>
	/// Configures the binding to resolve instances of the key type itself.
	/// </summary>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding ToSelf();

	/// <summary>
	/// Configures the binding to resolve instances using a factory of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The factory type, which must implement <see cref="IFactory"/>.</typeparam>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding ToFactory<T>() where T : IFactory;

	/// <summary>
	/// Configures the binding to resolve instances using the specified factory.
	/// </summary>
	/// <param name="factory">The factory to use for instance creation.</param>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding ToFactory(IFactory factory);

	/// <summary>
	/// Configures the binding to resolve instances using the specified factory function.
	/// </summary>
	/// <param name="factory">The factory function that creates instances of the binding type.</param>
	/// <returns>The current binding instance for chaining.</returns>
	IDiBinding ToFactory(Func<object> factory);

	/// <summary>
	/// Configures the binding to use a custom instance provider
	/// </summary>
	/// <remarks>
	/// This method is primarily intended for custom functionality extensions,
	/// allowing custom instance creation logic through <see cref="IInstanceProvider"/> implementation
	/// </remarks>
	/// <param name="instanceProvider">Custom instance provider implementing <see cref="IInstanceProvider"/></param>
	/// <returns>Current binding instance for chaining</returns>
	IDiBinding ToInstanceProvider(IInstanceProvider provider);

	/// <summary>
	/// Configures the binding to provide a single shared instance (singleton) for all resolutions.
	/// </summary>
	void AsSingle();

	/// <summary>
	/// Configures the binding to provide a new instance per scope.
	/// </summary>
	void AsScoped();

	/// <summary>
	/// Configures the binding to provide a new instance for every resolution (transient).
	/// </summary>
	void AsTransient();
}

}