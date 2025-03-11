using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using E314.DataTypes;
using E314.Exceptions;
using NUnit.Framework;

namespace E314.DI.Tests
{

internal sealed class DiContainerTests
{
	private IDiContainer _container;

	[SetUp]
	public void Setup()
	{
		_container = new DiContainer();
	}

	#region Bind

	[Test]
	public void Bind_DiBinding()
	{
		// Act
		var binding = _container.Bind<string>();

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.That(binding, Is.InstanceOf<IDiBinding>());
	}

	[Test]
	public void Bind_Bind_NotEqual()
	{
		// Act
		var binding = _container.Bind<string>();
		var bindingToo = _container.Bind<string>();

		// Assert
		Assert.That(binding, Is.EqualTo(bindingToo));
	}

	[Test]
	public void Bind_To_Bind_Equal()
	{
		// Act
		var binding = _container
			.Bind<ITestObject>()
			.To<TestObject>();

		var bindingToo = _container.Bind<ITestObject>();

		// Assert
		Assert.That(binding, Is.EqualTo(bindingToo));
	}

	#endregion

	#region Resolve

	[Test]
	public void Resolve_Exception()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _container.Resolve(null));
		Assert.Throws<InvOpException>(() => _container.Resolve(typeof(string)));
		Assert.Throws<InvOpException>(() => _container.Resolve<string>());
	}

	[Test]
	public void Bind_To_AsTransient_Resolve()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsTransient();

		var actual = _container.Resolve(typeof(ITestObject));

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void Bind_To_AsTransient_ResolveGeneric()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsTransient();

		var actual = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(actual, Is.Not.Null);
	}

	[Test]
	public void Bind_To_AsScoped_Resolve()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsScoped();

		var actual = _container.Resolve(typeof(ITestObject));

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void Bind_To_AsScoped_ResolveGeneric()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsScoped();

		var actual = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(actual, Is.Not.Null);
	}

	[Test]
	public void Bind_To_AsSingle_Resolve()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsSingle();

		var actual = _container.Resolve(typeof(ITestObject));

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void Bind_To_AsSingle_ResolveGeneric()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.AsSingle();

		var actual = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(actual, Is.Not.Null);
	}

	[Test]
	public void Bind_To_To_AsTransient_Resolve_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsTransient();

		var readOnlyList = _container.Resolve(typeof(IReadOnlyList<ITestObject>));
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(enumerable, Is.Not.Empty);
	}

	[Test]
	public void Bind_To_To_AsTransient_ResolveGeneric_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsTransient();

		var readOnlyList = _container.Resolve<IReadOnlyList<ITestObject>>();
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.Not.Empty);
	}

	[Test]
	public void Bind_To_To_AsScoped_Resolve_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsScoped();

		var readOnlyList = _container.Resolve(typeof(IReadOnlyList<ITestObject>));
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(enumerable, Is.Not.Empty);
	}

	[Test]
	public void Bind_To_To_AsScoped_ResolveGeneric_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsScoped();

		var readOnlyList = _container.Resolve<IReadOnlyList<ITestObject>>();
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.Not.Empty);
	}

	[Test]
	public void Bind_To_To_AsSingle_Resolve_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsSingle();

		var readOnlyList = _container.Resolve(typeof(IReadOnlyList<ITestObject>));
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.InstanceOf<IReadOnlyList<ITestObject>>());
		Assert.That(enumerable, Is.Not.Empty);
	}

	[Test]
	public void Bind_To_To_AsSingle_ResolveGeneric_Collection()
	{
		// Act
		_container.Bind<ITestObject>()
			.To<TestObject>()
			.To<TestObjectToo>()
			.AsSingle();

		var readOnlyList = _container.Resolve<IReadOnlyList<ITestObject>>();
		var enumerable = _container.Resolve(typeof(IEnumerable<ITestObject>));

		// Assert
		Assert.That(readOnlyList, Is.Not.Null);
		Assert.That(readOnlyList, Is.Not.Empty);
		Assert.That(enumerable, Is.Not.Null);
		Assert.That(enumerable, Is.Not.Empty);
	}

	#endregion

	#region Parent

	[Test]
	public void ChildBindTo_ParentResolve_Exception()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var binding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.Throws<InvOpException>(() => _ = parentContainer.Resolve<ITestObject>());
	}

	[Test]
	public void ParentBindTo_ChildBindTo_AreNotEqual()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var parentBinding = parentContainer
			.Bind<TestObject>()
			.ToSelf();

		var childBinding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		// Assert
		Assert.That(parentBinding, Is.Not.EqualTo(childBinding));
	}

	[Test]
	public void ParentBindToTo_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var obj = new TestObject();
		var objToo = new TestObjectToo();

		// Act
		parentContainer.Bind<ITestObject>()
			.To(obj)
			.To(objToo);

		var values = childContainer.Resolve<IReadOnlyList<ITestObject>>();
		var actualObj = values[0];
		var actualObjToo = values[1];

		// Assert
		Assert.That(actualObj, Is.EqualTo(obj));
		Assert.That(actualObjToo, Is.EqualTo(objToo));
		Assert.That(actualObj, Is.Not.EqualTo(objToo));
		Assert.That(actualObjToo, Is.Not.EqualTo(obj));
	}

	[Test]
	public void ParentBindTo_ChildBindTo_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var parentObj = new TestObject();
		var childObj = new TestObjectToo();

		// Act
		var parentBinding = parentContainer
			.Bind<ITestObject>()
			.To(parentObj);

		var childBinding = childContainer
			.Bind<ITestObject>()
			.To(childObj);

		var actualBinding = childContainer.GetBinding(typeof(ITestObject));

		// Assert
		Assert.That(actualBinding, Is.EqualTo(childBinding));
		Assert.That(actualBinding, Is.Not.EqualTo(parentBinding));
	}

	#endregion

	#region Parent_AsTransient

	[Test]
	public void ChildBindToAsTransient_ParentResolve_Exception()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var binding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		binding.AsTransient();

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.Throws<InvOpException>(() => _ = parentContainer.Resolve<ITestObject>());
	}

	[Test]
	public void ParentBindToAsTransient_ChildBindTo_AreNotEqual()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		parentContainer.Bind<TestObject>()
			.ToSelf()
			.AsTransient();

		var parentBinding = parentContainer.GetBinding(typeof(TestObject));

		var childBinding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		// Assert
		Assert.That(parentBinding, Is.Not.EqualTo(childBinding));
	}

	[Test]
	public void ParentBindToToAsTransient_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var obj = new TestObject();
		var objToo = new TestObjectToo();

		// Act
		parentContainer.Bind<ITestObject>()
			.To(obj)
			.To(objToo)
			.AsTransient();

		var values = childContainer.Resolve<IReadOnlyList<ITestObject>>();
		var actualObj = values[0];
		var actualObjToo = values[1];

		// Assert
		Assert.That(actualObj, Is.EqualTo(obj));
		Assert.That(actualObjToo, Is.EqualTo(objToo));
		Assert.That(actualObj, Is.Not.EqualTo(objToo));
		Assert.That(actualObjToo, Is.Not.EqualTo(obj));
	}

	[Test]
	public void ParentBindToAsTransient_ChildBindTo_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var parentObj = new TestObject();
		var childObj = new TestObjectToo();

		// Act
		var parentBinding = parentContainer
			.Bind<ITestObject>()
			.To(parentObj);

		parentBinding.AsTransient();

		childContainer.Bind<ITestObject>()
			.To(childObj)
			.AsTransient();

		var childBinding = childContainer.GetBinding(typeof(ITestObject));
		var actualBinding = childContainer.GetBinding(typeof(ITestObject));

		// Assert
		Assert.That(actualBinding, Is.EqualTo(childBinding));
		Assert.That(actualBinding, Is.Not.EqualTo(parentBinding));
	}

	#endregion

	#region Parent_AsSingle

	[Test]
	public void ChildBindToAsSingle_ParentResolve_Exception()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var binding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		binding.AsSingle();

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.Throws<InvOpException>(() => _ = parentContainer.Resolve<ITestObject>());
	}

	[Test]
	public void ParentBindToAsSingle_ChildBindTo_AreNotEqual()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var parentBinding = parentContainer
			.Bind<TestObject>()
			.ToSelf();

		parentBinding.AsSingle();

		var childBinding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		childBinding.AsSingle();

		// Assert
		Assert.That(parentBinding, Is.Not.EqualTo(childBinding));
	}

	[Test]
	public void ParentBindToToAsAsSingle_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var obj = new TestObject();
		var objToo = new TestObjectToo();

		// Act
		parentContainer.Bind<ITestObject>()
			.To(obj)
			.To(objToo)
			.AsSingle();

		var values = childContainer.Resolve<IReadOnlyList<ITestObject>>();
		var actualObj = values[0];
		var actualObjToo = values[1];

		// Assert
		Assert.That(actualObj, Is.EqualTo(obj));
		Assert.That(actualObjToo, Is.EqualTo(objToo));
		Assert.That(actualObj, Is.Not.EqualTo(objToo));
		Assert.That(actualObjToo, Is.Not.EqualTo(obj));
	}

	[Test]
	public void ParentBindToAsSingle_ChildBindToAsSingle_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var parentObj = new TestObject();
		var childObj = new TestObjectToo();

		// Act
		var parentBinding = parentContainer
			.Bind<ITestObject>()
			.To(parentObj);

		parentBinding.AsSingle();

		childContainer.Bind<ITestObject>()
			.To(childObj)
			.AsSingle();

		var childBinding = childContainer.GetBinding(typeof(ITestObject));
		var actualBinding = childContainer.GetBinding(typeof(ITestObject));

		// Assert
		Assert.That(actualBinding, Is.EqualTo(childBinding));
		Assert.That(actualBinding, Is.Not.EqualTo(parentBinding));
	}

	#endregion

	#region Parent_AsScoped

	[Test]
	public void ChildBindToAsScoped_ParentResolve_Exception()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var binding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		binding.AsScoped();

		// Assert
		Assert.That(binding, Is.Not.Null);
		Assert.Throws<InvOpException>(() => _ = parentContainer.Resolve<ITestObject>());
	}

	[Test]
	public void ParentBindToAsAsScoped_ChildBindToAsScoped_AreNotEqual()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		var parentBinding = parentContainer
			.Bind<TestObject>()
			.ToSelf();

		parentBinding.AsScoped();

		var childBinding = childContainer
			.Bind<TestObject>()
			.ToSelf();

		childBinding.AsScoped();

		// Assert
		Assert.That(parentBinding, Is.Not.EqualTo(childBinding));
	}

	[Test]
	public void ParentBindToToAsScoped_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var obj = new TestObject();
		var objToo = new TestObjectToo();

		// Act
		parentContainer.Bind<ITestObject>()
			.To(obj)
			.To(objToo)
			.AsScoped();

		var values = childContainer.Resolve<IReadOnlyList<ITestObject>>();
		var actualObj = values[0];
		var actualObjToo = values[1];

		// Assert
		Assert.That(actualObj, Is.EqualTo(obj));
		Assert.That(actualObjToo, Is.EqualTo(objToo));
		Assert.That(actualObj, Is.Not.EqualTo(objToo));
		Assert.That(actualObjToo, Is.Not.EqualTo(obj));
	}

	[Test]
	public void ParentBindToAsScoped_ChildBindToAsScoped_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var parentObj = new TestObject();
		var childObj = new TestObjectToo();

		// Act
		var parentBinding = parentContainer
			.Bind<ITestObject>()
			.To(parentObj);

		parentBinding.AsScoped();

		childContainer.Bind<ITestObject>()
			.To(childObj)
			.AsScoped();

		var childBinding = childContainer.GetBinding(typeof(ITestObject));
		var actualBinding = childContainer.GetBinding(typeof(ITestObject));

		// Assert
		Assert.That(actualBinding, Is.EqualTo(childBinding));
		Assert.That(actualBinding, Is.Not.EqualTo(parentBinding));
	}

	[Test]
	public void ParentBindToAsScoped_ChildResolve()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		parentContainer
			.Bind<ITestObject>()
			.To<TestObject>()
			.AsScoped();

		var childTestObject = childContainer.Resolve<ITestObject>();
		var childTestObjectToo = childContainer.Resolve<ITestObject>();
		var parentTestObject = parentContainer.Resolve<ITestObject>();
		var parentTestObjectToo = parentContainer.Resolve<ITestObject>();

		// Assert
		Assert.That(childTestObject, Is.EqualTo(childTestObjectToo));
		Assert.That(parentTestObject, Is.Not.EqualTo(childTestObject));
		Assert.That(parentTestObject, Is.Not.EqualTo(childTestObjectToo));
		Assert.That(parentTestObject, Is.EqualTo(parentTestObjectToo));
	}

	#endregion

	#region Disposable

	[Test]
	public void AsScoped_Disposable()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		parentContainer.Bind<TestObject>()
			.ToSelf()
			.AsScoped();

		var testObject = childContainer.Resolve<TestObject>();
		var parentTestObject = parentContainer.Resolve<TestObject>();
		childContainer.Dispose();

		// Assert
		Assert.That(testObject.IsEmpty, Is.True);
		Assert.That(parentTestObject.IsEmpty, Is.False);
	}

	[Test]
	public void AsSingle_Disposable()
	{
		// Arrange
		var container = new DiContainer();

		// Act
		container.Bind<TestObject>()
			.ToSelf()
			.AsSingle();

		var testObject = container.Resolve<TestObject>();
		container.Dispose();

		// Assert
		Assert.That(testObject.IsEmpty, Is.True);
	}

	[Test]
	public void AsTransient_Disposable()
	{
		// Arrange
		var container = new DiContainer();

		// Act
		container.Bind<TestObject>()
			.ToSelf()
			.AsTransient();

		var testObject = container.Resolve<TestObject>();
		container.Dispose();

		// Assert
		Assert.That(testObject.IsEmpty, Is.False);
	}

	#endregion

	#region InstanceProvider

	[Test]
	public void Bind_ToInstanceProvider_Resolve()
	{
		// Arrange
		var instance = new TestObject();
		var provider = new InstanceProvider(instance);

		// Act
		_container.Bind<ITestObject>()
			.ToInstanceProvider(provider);

		var actual = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.SameAs(instance));
	}

	[Test]
	public void Bind_ToInstanceProvider_Multiple_Resolve()
	{
		// Arrange
		var instance1 = new TestObject();
		var instance2 = new TestObjectToo();
		var provider1 = new InstanceProvider(instance1);
		var provider2 = new InstanceProvider(instance2);

		// Act
		_container.Bind<ITestObject>()
			.ToInstanceProvider(provider1)
			.ToInstanceProvider(provider2);

		var values = _container.Resolve<IReadOnlyList<ITestObject>>();

		// Assert
		Assert.That(values, Is.Not.Null);
		Assert.That(values, Has.Count.EqualTo(2));
		Assert.That(values[0], Is.SameAs(instance1));
		Assert.That(values[1], Is.SameAs(instance2));
	}

	[Test]
	public void Bind_ToInstanceProvider_Parent_Child_SameInstance()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);
		var instance = new TestObject();
		var provider = new InstanceProvider(instance);

		// Act
		parentContainer.Bind<ITestObject>()
			.ToInstanceProvider(provider);

		var parentInstance = parentContainer.Resolve<ITestObject>();
		var childInstance = childContainer.Resolve<ITestObject>();

		// Assert
		Assert.That(parentInstance, Is.Not.Null);
		Assert.That(childInstance, Is.Not.Null);
		Assert.That(parentInstance, Is.SameAs(instance));
		Assert.That(childInstance, Is.SameAs(instance));
		Assert.That(parentInstance, Is.SameAs(childInstance));
	}

	[Test]
	public void Bind_ToInstanceProvider_Null_ThrowsException()
	{
		// Act & Assert
		Assert.Throws<ArgNullException>(() => _container.Bind<ITestObject>().ToInstanceProvider(null));
	}

	#endregion

	#region Factory

	[Test]
	public void Bind_ToFactory_Func_Resolve()
	{
		// Act
		_container.Bind<ITestObject>()
			.ToFactory(() => new TestObject());

		var actual = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual, Is.InstanceOf<TestObject>());
	}

	[Test]
	public void Bind_ToFactory_Func_WithDependency_Resolve()
	{
		// Arrange
		var dependency = new TestObjectToo();
		_container.Bind<ITestObject>()
			.To(dependency);
		
		// Act
		_container.Bind<ITestObjectWithDependency>()
			.ToFactory(() => {
				var dep = _container.Resolve<ITestObject>();
				return new TestObjectWithDependency(dep);
			});

		var actual = _container.Resolve<ITestObjectWithDependency>();

		// Assert
		Assert.That(actual, Is.Not.Null);
		Assert.That(actual.Dependency, Is.SameAs(dependency));
	}

	[Test]
	public void Bind_ToFactory_Func_Multiple_Resolve()
	{
		// Act
		_container.Bind<ITestObject>()
			.ToFactory(() => new TestObject())
			.ToFactory(() => new TestObjectToo());

		var values = _container.Resolve<IReadOnlyList<ITestObject>>();

		// Assert
		Assert.That(values, Is.Not.Null);
		Assert.That(values, Has.Count.EqualTo(2));
		Assert.That(values[0], Is.InstanceOf<TestObject>());
		Assert.That(values[1], Is.InstanceOf<TestObjectToo>());
	}

	[Test]
	public void Bind_ToFactory_Func_AsTransient_NewInstance()
	{
		// Act
		_container.Bind<ITestObject>()
			.ToFactory(() => new TestObject())
			.AsTransient();

		var instance1 = _container.Resolve<ITestObject>();
		var instance2 = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(instance1, Is.Not.Null);
		Assert.That(instance2, Is.Not.Null);
		Assert.That(instance1, Is.Not.SameAs(instance2));
	}

	[Test]
	public void Bind_ToFactory_Func_AsSingle_SameInstance()
	{
		// Act
		_container.Bind<ITestObject>()
			.ToFactory(() => new TestObject())
			.AsSingle();

		var instance1 = _container.Resolve<ITestObject>();
		var instance2 = _container.Resolve<ITestObject>();

		// Assert
		Assert.That(instance1, Is.Not.Null);
		Assert.That(instance2, Is.Not.Null);
		Assert.That(instance1, Is.SameAs(instance2));
	}

	[Test]
	public void Bind_ToFactory_Func_AsScoped_Parent_Child_DifferentInstances()
	{
		// Arrange
		var parentContainer = new DiContainer();
		var childContainer = new DiContainer(bindingProvider: parentContainer);

		// Act
		parentContainer.Bind<ITestObject>()
			.ToFactory(() => new TestObject())
			.AsScoped();

		var parentInstance = parentContainer.Resolve<ITestObject>();
		var childInstance = childContainer.Resolve<ITestObject>();

		// Assert
		Assert.That(parentInstance, Is.Not.Null);
		Assert.That(childInstance, Is.Not.Null);
		Assert.That(parentInstance, Is.Not.SameAs(childInstance));
	}

	#endregion

	#region Nested

	private interface ITestObject
	{
	}

	private interface ITestObjectWithDependency
	{
		ITestObject Dependency { get; }
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class TestObject : ITestObject,
		IDisposable
	{
		public bool IsEmpty
		{
			get;
			private set;
		}

		public void Dispose()
		{
			IsEmpty = true;
		}
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class TestObjectToo : ITestObject
	{
	}

	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class TestObjectWithDependency : ITestObjectWithDependency
	{
		public ITestObject Dependency { get; }

		public TestObjectWithDependency(ITestObject dependency)
		{
			Dependency = dependency;
		}
	}

	#endregion
}

}