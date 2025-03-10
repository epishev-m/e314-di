using System;
using System.Linq;
using E314.DataTypes;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DI.Tests
{

internal sealed class DiBindingTests
{
	private ICapacityStrategy _capacityStrategy;
	private ITypeAnalyzer _analyzer;
	private IDiContainer _container;
	private Type _key;
	private DiBinding _binding;

	[SetUp]
	public void Setup()
	{
		_capacityStrategy = new CapacityStrategy();
		_analyzer = new TypeAnalyzer();
		_container = new DiContainer();
		_key = typeof(TestObject);
		_binding = new DiBinding(_capacityStrategy, _analyzer, _container, _key);
	}

	#region Constructor

	[Test]
	public void Constructor_NotException()
	{
		// Act & Assert
		Assert.DoesNotThrow(() => _ = new DiBinding(_capacityStrategy, _analyzer, _container, _key));
	}

	[Test]
	public void Constructor_Exception()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _ = new DiBinding(null, _analyzer, _container, _key));
		Assert.Throws<ArgNullException>(() => _ = new DiBinding(_capacityStrategy, null, _container, _key));
		Assert.Throws<ArgNullException>(() => _ = new DiBinding(_capacityStrategy, _analyzer, null, _key));
		Assert.Throws<ArgNullException>(() => _ = new DiBinding(_capacityStrategy, _analyzer, _container, null));
	}

	#endregion

	#region InstanceProvider

	[Test]
	public void To_InstanceProvider()
	{
		// Act
		var binding = _binding.To(new TestObject());
		var instanceProvider = _binding.Values[0];

		// Assert
		Assert.That(binding, Is.EqualTo(_binding));
		Assert.That(instanceProvider, Is.InstanceOf<InstanceProvider>());
	}

	[Test]
	public void ToGeneric_InstanceProvider()
	{
		// Act
		var binding = _binding.To<TestObject>();
		var instanceProvider = _binding.Values[0];

		// Assert
		Assert.That(binding, Is.EqualTo(_binding));
		Assert.That(instanceProvider, Is.InstanceOf<ActivatorInstanceProvider>());
	}

	[Test]
	public void ToSelf_InstanceProvider()
	{
		// Act
		var binding = _binding.ToSelf();
		var instanceProvider = _binding.Values[0];

		// Assert
		Assert.That(binding, Is.EqualTo(_binding));
		Assert.That(instanceProvider, Is.InstanceOf<ActivatorInstanceProvider>());
	}

	[Test]
	public void ToFactory_InstanceProvider()
	{
		// Arrange
		var factory = new TestFactory();

		// Act
		var binding = _binding.ToFactory(factory);
		var instanceProvider = _binding.Values[0];

		// Assert
		Assert.That(binding, Is.EqualTo(_binding));
		Assert.That(instanceProvider, Is.InstanceOf<FactoryInstanceProvider>());
	}

	[Test]
	public void ToFunc_InstanceProvider()
	{
		// Arrange
		var obj = new TestObject();

		// Act
		var binding = _binding.ToFactory(Func);
		var instanceProvider = _binding.Values[0];

		// Assert
		Assert.That(binding, Is.EqualTo(_binding));
		Assert.That(instanceProvider, Is.InstanceOf<FactoryInstanceProvider>());
		return;

		object Func() => obj;
	}

	#endregion

	#region AsTransient

	[Test]
	public void To_AsTransient()
	{
		// Arrange
		var expected = new TestObject();

		// Act
		_binding.To(expected)
			.AsTransient();
		var instanceProvider = _binding.Values[0];
		var actual = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<InstanceProvider>());
		Assert.That(actual, Is.InstanceOf<TestObject>());
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void ToGeneric_AsTransient()
	{
		// Act
		_binding.To<TestObject>()
			.AsTransient();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ActivatorInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToSelf_AsTransient()
	{
		// Act
		_binding.ToSelf()
			.AsTransient();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ActivatorInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToFactory_AsTransient()
	{
		// Arrange
		var factory = new TestFactory();

		// Act
		_binding.ToFactory(factory)
			.AsTransient();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<FactoryInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToFunc_AsTransient()
	{
		// Arrange
		var obj = new TestObject();

		// Act
		_binding.ToFactory(Func)
			.AsTransient();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<FactoryInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instance, Is.EqualTo(obj));
		return;

		object Func() => obj;
	}

	#endregion

	#region AsScoped

	[Test]
	public void To_AsScoped()
	{
		// Arrange
		var expected = new TestObject();

		// Act
		_binding.To(expected)
			.AsScoped();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ScopeInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instance, Is.EqualTo(expected));
		Assert.That(instance, Is.EqualTo(instanceToo));
	}

	[Test]
	public void ToGeneric_AsScoped()
	{
		// Act
		_binding.To<TestObject>()
			.AsScoped();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ScopeInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToSelf_AsScoped()
	{
		// Act
		_binding.ToSelf()
			.AsScoped();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ScopeInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToFactory_AsScope()
	{
		// Arrange
		var factory = new TestFactory();

		// Act
		_binding.ToFactory(factory)
			.AsScoped();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ScopeInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void ToFunc_AsScope()
	{
		// Act
		_binding.ToFactory(Func)
			.AsScoped();
		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<ScopeInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		return;

		object Func() => new TestObject();
	}

	#endregion

	#region AsSingle

	[Test]
	public void To_AsSingle()
	{
		// Arrange
		var expected = new TestObject();

		// Act
		_binding.To(expected)
			.AsSingle();

		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<SingletonInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instance, Is.EqualTo(expected));
		Assert.That(instanceToo, Is.EqualTo(expected));
	}

	[Test]
	public void ToGeneric_AsSingle()
	{
		// Act
		_binding.To<TestObject>()
			.AsSingle();

		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<SingletonInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instanceToo, Is.EqualTo(instance));
	}

	[Test]
	public void ToSelf_AsSingle()
	{
		// Act
		_binding.ToSelf()
			.AsSingle();

		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<SingletonInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instanceToo, Is.EqualTo(instance));
	}

	[Test]
	public void ToFactory_AsSingle()
	{
		// Arrange
		var factory = new TestFactory();

		// Act
		_binding.ToFactory(factory)
			.AsSingle();

		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<SingletonInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instanceToo, Is.EqualTo(instance));
	}

	[Test]
	public void ToFunc_AsSingle()
	{
		// Act
		_binding.ToFactory(Func)
			.AsSingle();

		var instanceProvider = _binding.Values[0];
		var instance = instanceProvider.GetInstance();
		var instanceToo = instanceProvider.GetInstance();

		// Assert
		Assert.That(instanceProvider, Is.InstanceOf<SingletonInstanceProvider>());
		Assert.That(instance, Is.InstanceOf<TestObject>());
		Assert.That(instanceToo, Is.EqualTo(instance));
		return;

		object Func() => new TestObject();
	}

	#endregion

	#region Other

	[Test]
	public void Values_Empty()
	{
		// Act
		var values = _binding.Values;

		// Assert
		Assert.That(values, Is.Empty);
	}

	[Test]
	public void To_Values_NotEmpty()
	{
		// Act
		_binding.To(new TestObject());
		var values = _binding.Values;

		// Assert
		Assert.That(values, Is.Not.Empty);
	}

	[Test]
	public void To_Values()
	{
		// Arrange
		var obj = new TestObject();
		var objToo = new TestObject();

		// Act
		_binding.To(obj)
			.To(objToo);

		var values = _binding.Values.ToList();
		var actualObj = values[0].GetInstance();
		var actualObjToo = values[1].GetInstance();

		// Assert
		Assert.That(actualObj, Is.EqualTo(obj));
		Assert.That(actualObjToo, Is.EqualTo(objToo));
		Assert.That(actualObj, Is.Not.EqualTo(objToo));
		Assert.That(actualObjToo, Is.Not.EqualTo(obj));
	}

	#endregion

	#region Nested

	private sealed class TestObject
	{
	}

	private sealed class TestFactory : IFactory
	{
		public object Create()
		{
			return new TestObject();
		}

		public void Dispose()
		{
		}
	}

	#endregion
}

}