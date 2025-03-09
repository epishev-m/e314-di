using E314.DataTypes;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DI.Tests
{

internal sealed class ScopeInstanceProviderTests
{
	[Test]
	public void Constructor()
	{
		// Arrange
		var instanceProvider = new TestInstanceProvider(null);

		// Act & Assert
		Assert.DoesNotThrow(() => _ = new ScopeInstanceProvider(instanceProvider));
		Assert.Throws<ArgNullException>(() => _ = new ScopeInstanceProvider(null));
	}

	[Test]
	public void GetInstance()
	{
		// Arrange
		var obj = new object();
		var instanceProvider = new TestInstanceProvider(obj);
		var scopeInstanceProvider = new ScopeInstanceProvider(instanceProvider);

		// Act
		var actual = scopeInstanceProvider.GetInstance();

		// Assert
		Assert.That(actual, Is.EqualTo(obj));
	}

	[Test]
	public void Disposable()
	{
		// Arrange
		var instanceProvider = new TestInstanceProvider(null);
		var scopeInstanceProvider = new ScopeInstanceProvider(instanceProvider);

		// Act
		scopeInstanceProvider.Dispose();

		// Assert
		Assert.That(instanceProvider.IsEmpty, Is.True);
	}

	#region Nested

	private sealed class TestInstanceProvider : IInstanceProvider
	{
		private readonly object _obj;

		public bool IsEmpty
		{
			get;
			private set;
		}

		public TestInstanceProvider(object obj)
		{
			_obj = obj;
		}

		public object GetInstance()
		{
			return _obj;
		}

		public void Dispose()
		{
			IsEmpty = true;
		}
	}

	#endregion
}

}