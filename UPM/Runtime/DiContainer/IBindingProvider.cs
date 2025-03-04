using System;

namespace E314.DI
{

/// <summary>
/// Provides access to bindings for types.
/// </summary>
public interface IBindingProvider
{
	/// <summary>
	/// Retrieves the binding for the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve the binding for.</param>
	/// <returns>The binding for the specified type, or null if no binding exists.</returns>
	IDiBinding GetBinding(Type type);
}

}