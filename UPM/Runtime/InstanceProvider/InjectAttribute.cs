using System;

namespace E314.DI
{

/// <summary>
/// Marks a constructor as injectable, indicating that it should be used for dependency injection.
/// This attribute is typically used to identify the primary constructor for creating instances
/// in dependency injection frameworks.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public class InjectAttribute : Attribute
{
}

}