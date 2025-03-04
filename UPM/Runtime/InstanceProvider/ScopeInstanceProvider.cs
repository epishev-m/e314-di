using E314.DataTypes;
using E314.Protect;

namespace E314.DI
{

/// <summary>
/// Represents a scoped instance provider that wraps another instance provider.
/// Ensures proper disposal of resources and delegates instance retrieval to the underlying provider.
/// </summary>
public sealed class ScopeInstanceProvider : IInstanceProvider
{
	private readonly IInstanceProvider _instanceProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="ScopeInstanceProvider"/> class.
	/// </summary>
	/// <param name="instanceProvider">The underlying instance provider to wrap.</param>
	/// <exception cref="ArgNullException">Thrown when <paramref name="instanceProvider"/> is null.</exception>
	public ScopeInstanceProvider(IInstanceProvider instanceProvider)
	{
		Requires.NotNull(instanceProvider, nameof(instanceProvider));
		_instanceProvider = instanceProvider;
	}

	/// <summary>
	/// Disposes of the resources used by the underlying instance provider.
	/// </summary>
	public void Dispose()
	{
		_instanceProvider.Dispose();
	}

	/// <summary>
	/// Retrieves an instance from the underlying instance provider.
	/// </summary>
	/// <returns>The instance provided by the underlying instance provider.</returns>
	public object GetInstance()
	{
		return _instanceProvider.GetInstance();
	}
}

}