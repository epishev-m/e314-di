using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using E314.DataTypes;
using E314.Protect;

namespace E314.DI
{

/// <summary>
/// A dependency injection container implementation that supports binding, resolving, and scoping.
/// </summary>
public sealed class DiContainer : Binder<Type, IInstanceProvider>, IDiContainer, IBindingProvider
{
	/// <summary>
	/// Represents the configuration for the dependency injection container.
	/// </summary>
	public readonly struct Config
	{
		/// <summary>
		/// The initial capacity of the container.
		/// </summary>
		public readonly int Capacity;

		/// <summary>
		/// The initial capacity for scoped instances.
		/// </summary>
		public readonly int ScopeCapacity;

		/// <summary>
		/// Initializes a new instance of the <see cref="Config"/> structure.
		/// </summary>
		/// <param name="capacity">The initial capacity of the container.</param>
		/// <param name="scopeCapacity">The initial capacity for scoped instances.</param>
		public Config(int capacity = 127, int scopeCapacity = 7)
		{
			Requires.InRange(capacity, 1, int.MaxValue, nameof(capacity));
			Requires.InRange(scopeCapacity, 1, int.MaxValue, nameof(scopeCapacity));
			Capacity = capacity;
			ScopeCapacity = scopeCapacity;
		}
	}

	private readonly IBindingProvider _bindingProvider;
	private readonly Dictionary<Type, object> _scopeInstances;
	private readonly ITypeAnalyzer _typeAnalyzer = new TypeAnalyzer();

	/// <summary>
	/// Initializes a new instance of the <see cref="DiContainer"/> class.
	/// </summary>
	/// <param name="config">The configuration for the container.</param>
	/// <param name="bindingProvider">An optional binding provider for additional bindings.</param>
	public DiContainer(Config config = default, IBindingProvider bindingProvider = null)
		: base(new CapacityStrategy(), config.Capacity)
	{
		_bindingProvider = bindingProvider;
		_scopeInstances = new Dictionary<Type, object>(config.ScopeCapacity);
	}

	/// <summary>
	/// Creates a binding for the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to bind.</typeparam>
	/// <returns>A binding configuration for the specified type.</returns>
	public IDiBinding Bind<T>()
	{
		return Bind(typeof(T));
	}

	/// <summary>
	/// Creates a binding for the specified type.
	/// </summary>
	/// <param name="type">The type to bind.</param>
	/// <returns>A binding configuration for the specified type.</returns>
	public new IDiBinding Bind(Type type)
	{
		return base.Bind(type) as IDiBinding;
	}

	/// <summary>
	/// Resolves an instance of the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type to resolve.</typeparam>
	/// <returns>An instance of the specified type.</returns>
	public T Resolve<T>()
	{
		Requires.NotDisposed(IsDisposed);
		return (T) Resolve(typeof(T));
	}

	/// <summary>
	/// Resolves an instance of the specified type.
	/// </summary>
	/// <param name="type">The type to resolve.</param>
	/// <returns>An instance of the specified type.</returns>
	public object Resolve(Type type)
	{
		Requires.NotNull(type, nameof(type));
		return IsSupportedCollection(type)
			? ResolveCollection(type) 
			: ResolveSingle(type);
	}

	/// <summary>
	/// Retrieves the binding for the specified type.
	/// </summary>
	/// <param name="type">The type to retrieve the binding for.</param>
	/// <returns>The binding for the specified type, or null if no binding exists.</returns>
	public new IDiBinding GetBinding(Type type)
	{
		return base.GetBinding(type) as IDiBinding;
	}

	/// <summary>
	/// Releases all resources used by the container, including scoped instances.
	/// </summary>
	public override void Dispose()
	{
		foreach (var instance in _scopeInstances.Values)
		{
			if (instance is IDisposable disposable) disposable.Dispose();
		}

		_scopeInstances.Clear();
		base.Dispose();
	}

	/// <summary>
	/// Creates a raw binding for the specified key type.
	/// </summary>
	/// <param name="key">The key type for the binding.</param>
	/// <returns>A new binding instance.</returns>
	protected override IBinding<Type, IInstanceProvider> GetRawBinding(Type key)
	{
		return new DiBinding(CapacityStrategy, _typeAnalyzer, this, key);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsSupportedCollection(Type type)
	{
		if (!type.IsGenericType) return false;
		var openGenericType = type.GetGenericTypeDefinition();
		return openGenericType == typeof(IEnumerable<>) || openGenericType == typeof(IReadOnlyList<>);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private object ResolveCollection(Type type)
	{
		var contentType = type.GetGenericArguments()[0];
		var binding = GetBinding(contentType) ?? _bindingProvider?.GetBinding(contentType);
		Requires.Ensure(binding != null, $"Type {type} is not registered!");
		if (!binding!.IsScopeInstance) return BindingToList(binding);
		if (_scopeInstances.TryGetValue(type, out var instance)) return instance;
		instance = BindingToList(binding);
		_scopeInstances.Add(type, instance);
		return instance;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private object ResolveSingle(Type type)
	{
		var binding = GetBinding(type) ?? _bindingProvider?.GetBinding(type);
		Requires.Ensure(binding != null, $"Type {type} is not registered!");
		if (!binding!.IsScopeInstance) return binding.Values[0].GetInstance();
		if (_scopeInstances.TryGetValue(type, out var instance)) return instance;
		var instanceProvider = binding.Values[0];
		instance = instanceProvider.GetInstance();
		_scopeInstances.Add(type, instance);
		return instance;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IList BindingToList(IDiBinding binding)
	{
		var genericType = typeof(List<>).MakeGenericType(binding.Key);
		var resultList = (IList)Activator.CreateInstance(genericType, binding.Values.Count);
		FillResultList(resultList, binding.Values);
		return resultList;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void FillResultList(IList resultList, IReadOnlyList<IInstanceProvider> values)
	{
		foreach (var instanceProvider in values)
		{
			var instance = instanceProvider.GetInstance();
			resultList!.Add(instance);
		}
	}
}

}