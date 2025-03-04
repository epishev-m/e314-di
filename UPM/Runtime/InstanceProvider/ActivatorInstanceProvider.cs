using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using E314.DataTypes;
using E314.Protect;

namespace E314.DI
{

/// <summary>
/// Provides instances of a type by using reflection to invoke its constructors.
/// Supports dependency injection via constructor parameters resolved from a DI container.
/// </summary>
public sealed class ActivatorInstanceProvider : IInstanceProvider
{
	private static readonly Dictionary<int, bool> Cache = new(32);
	private readonly Type _type;
	private readonly ITypeAnalyzer _typeAnalyzer;
	private readonly IDiContainer _container;

	/// <summary>
	/// Initializes a new instance of the <see cref="ActivatorInstanceProvider"/> class.
	/// </summary>
	/// <param name="type">The type for which instances will be created.</param>
	/// <param name="typeAnalyzer">The analyzer used to inspect the type's constructors.</param>
	/// <param name="container">The dependency injection container used to resolve constructor parameters.</param>
	/// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
	public ActivatorInstanceProvider(Type type,
		ITypeAnalyzer typeAnalyzer,
		IDiContainer container)
	{
		Requires.NotNull(type, nameof(type));
		Requires.NotNull(typeAnalyzer, nameof(typeAnalyzer));
		Requires.NotNull(container, nameof(container));
		_type = type;
		_typeAnalyzer = typeAnalyzer;
		_container = container;
	}

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
	/// </summary>
	public void Dispose()
	{
	}

	/// <summary>
	/// Creates and returns an instance of the type managed by this provider.
	/// </summary>
	/// <returns>An instance of the type, created by invoking the appropriate constructor.</returns>
	public object GetInstance()
	{
		var typeAnalysisResult = _typeAnalyzer.Analyze(_type, TypeAnalysisFlags.Constructors);
		var constructInfo = typeAnalysisResult.Constructors.Count == 1
			? typeAnalysisResult.Constructors[0]
			: GetPrimaryConstructor(typeAnalysisResult.Constructors);
		var args = constructInfo.GetParameters()
			.Select(info => _container.Resolve(info.ParameterType))
			.ToArray();
		return constructInfo.Invoke(args);
	}

	/// <summary>
	/// Identifies the primary constructor to use for creating instances.
	/// The primary constructor is either marked with the <see cref="InjectAttribute"/> or is the first available constructor.
	/// </summary>
	/// <param name="constructorsInfos">A list of available constructors for the type.</param>
	/// <returns>The primary constructor to use.</returns>
	/// <exception cref="InvalidOperationException">Thrown if multiple constructors are marked with the <see cref="InjectAttribute"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ConstructorInfo GetPrimaryConstructor(IReadOnlyList<ConstructorInfo> constructorsInfos)
	{
		var constructors = constructorsInfos.Where(IsInject).ToArray();
		if (constructors.Length == 0) return constructorsInfos[0];
		Requires.Ensure(constructors.Length <= 1, $"Type found multiple [Inject] marked constructors, type: {_type.Name}");
		return constructors[0];
	}

	/// <summary>
	/// Determines whether a constructor is marked with the <see cref="InjectAttribute"/>.
	/// Results are cached for performance optimization.
	/// </summary>
	/// <param name="constructorInfo">The constructor to check.</param>
	/// <returns><c>true</c> if the constructor is marked with the <see cref="InjectAttribute"/>; otherwise, <c>false</c>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsInject(ConstructorInfo constructorInfo)
	{
		var hashCode = constructorInfo.GetHashCode();
		if (Cache.TryGetValue(hashCode, out var value)) return value;
		value = constructorInfo.IsDefined(typeof(InjectAttribute), false);
		Cache.Add(hashCode, value);
		return value;
	}
}

}