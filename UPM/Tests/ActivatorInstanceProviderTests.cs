using System;
using System.Diagnostics.CodeAnalysis;
using E314.DataTypes;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DI.Tests
{

internal sealed class ActivatorInstanceProviderTests
{
	private ITypeAnalyzer _analyzer;
	private IDiContainer _container;

	[SetUp]
	public void Setup()
	{
		_analyzer = new TypeAnalyzer();
		_container = new DiContainer();
	}

	[Test]
	public void Constructor_NotException()
	{
		// Act & Assert
		Assert.DoesNotThrow(() => _ = new ActivatorInstanceProvider(typeof(TestObjectDefault), _analyzer, _container));
	}

	[Test]
	public void Constructor_Exception()
	{
		// Arrange
		var type = typeof(TestObjectDefault);

		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new ActivatorInstanceProvider(null, _analyzer, _container));
		Assert.Throws<ArgNullException>(() => _ = new ActivatorInstanceProvider(type, null, _container));
		Assert.Throws<ArgNullException>(() => _ = new ActivatorInstanceProvider(type, _analyzer, null));
	}

	[Test]
	public void GetInstance_ConstructorDefault()
	{
		// Arrange
		var instanceProvider = new ActivatorInstanceProvider(typeof(TestObjectDefault), _analyzer, _container);

		// Act
		var actual = instanceProvider.GetInstance();

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObjectDefault>());
	}

	[Test]
	public void GetInstance_ConstructorSingle()
	{
		// Arrange
		_container.Bind<TestObjectDefault>()
			.ToSelf()
			.AsTransient();

		var instanceProvider = new ActivatorInstanceProvider(typeof(TestObjectSingle), _analyzer, _container);

		// Act
		var actual = instanceProvider.GetInstance() as TestObjectSingle;

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObjectSingle>());
		Assert.That(actual.Value, Is.EqualTo(1));
	}

	[Test]
	public void GetInstance_ConstructorMultiple()
	{
		// Arrange
		_container.Bind<TestObjectDefault>()
			.ToSelf()
			.AsTransient();

		var instanceProvider = new ActivatorInstanceProvider(typeof(TestObjectMultiple), _analyzer, _container);

		// Act
		var actual = instanceProvider.GetInstance() as TestObjectMultiple;

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObjectMultiple>());
	}

	[Test]
	public void GetInstance_ConstructorInject()
	{
		// Arrange
		_container.Bind<TestObjectDefault>()
			.ToSelf()
			.AsTransient();

		var instanceProvider = new ActivatorInstanceProvider(typeof(TestObjectInject), _analyzer, _container);

		// Act
		var actual = instanceProvider.GetInstance() as TestObjectInject;

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObjectInject>());
		Assert.That(actual.Value, Is.EqualTo(1));
	}

	[Test]
	public void GetInstance_ConstructorMultipleInject()
	{
		// Arrange
		_container.Bind<TestObjectDefault>()
			.ToSelf()
			.AsTransient();

		var instanceProvider = new ActivatorInstanceProvider(typeof(TestObjectMultipleInject), _analyzer, _container);

		// Act & Assert
		Assert.Throws<InvOpException>(() => _ = instanceProvider.GetInstance());
	}

	#region Nested

	private sealed class TestObjectDefault
	{
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private sealed class TestObjectSingle
	{
		public readonly int Value;

		public TestObjectSingle(TestObjectDefault param)
		{
			Value = 1;
		}
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private sealed class TestObjectMultiple
	{
		public TestObjectMultiple(TestObjectDefault param)
		{
		}

		public TestObjectMultiple(TestObjectDefault param1, TestObjectDefault param2)
		{
		}
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private sealed class TestObjectInject
	{
		public readonly int Value;

		[Inject]
		public TestObjectInject(TestObjectDefault param)
		{
			Value = 1;
		}

		public TestObjectInject(TestObjectDefault param1, TestObjectDefault param2)
		{
			Value = 2;
		}
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private sealed class TestObjectMultipleInject
	{
		public readonly int Value;

		[Inject]
		public TestObjectMultipleInject(TestObjectDefault param)
		{
			Value = 1;
		}

		[Inject]
		public TestObjectMultipleInject(TestObjectDefault param1, TestObjectDefault param2)
		{
			Value = 2;
		}
	}

	#endregion
}

}

