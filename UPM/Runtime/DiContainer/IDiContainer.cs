using System;

namespace E314.DI
{

/// <summary>
/// Represents a dependency injection container that manages bindings and resolves instances.
/// </summary>
public interface IDiContainer
{
	/// <summary>
	/// Creates a binding for the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to bind.</typeparam>
	/// <returns>A binding configuration for the specified type.</returns>
	IDiBinding Bind<T>();

	/// <summary>
	/// Creates a binding for the specified type.
	/// </summary>
	/// <param name="type">The type to bind.</param>
	/// <returns>A binding configuration for the specified type.</returns>
	IDiBinding Bind(Type type);

	/// <summary>
	/// Resolves an instance of the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to resolve.</typeparam>
	/// <returns>An instance of the specified type.</returns>
	T Resolve<T>();

	/// <summary>
	/// Resolves an instance of the specified type.
	/// </summary>
	/// <param name="type">The type to resolve.</param>
	/// <returns>An instance of the specified type.</returns>
	object Resolve(Type type);
}

}