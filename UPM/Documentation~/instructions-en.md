# E314.DI

## Description

The `E314.DI` module contains the `DiContainer`.
`DiContainer` is a dependency injection (DI) container that helps manage object creation and their dependencies. In this guide, you will learn how to configure the container, bind types, resolve dependencies, and manage their lifecycle.

## Table of Contents

- [E314.DI](#e314di)
  - [Description](#description)
  - [Table of Contents](#table-of-contents)
  - [Core Concepts](#core-concepts)
  - [Container Configuration](#container-configuration)
  - [Creating Bindings](#creating-bindings)
    - [Nested Containers](#nested-containers)
    - [Types with Multiple Constructors](#types-with-multiple-constructors)
  - [Dependency Resolution](#dependency-resolution)
  - [Lifecycle Management](#lifecycle-management)
    - [Transient (AsTransient)](#transient-astransient)
    - [Singleton (AsSingle)](#singleton-assingle)
    - [Scoped (AsScoped)](#scoped-asscoped)
  - [Recommendations](#recommendations)

## Core Concepts

**Dependency Injection (DI**) is a technique that separates object creation from their usage. Instead of creating dependencies within a class, they are provided (injected) from the outside.

**DI Container** is a component that:

- Manages the registration (binding) of types and their implementations.
- Creates objects (instances) and injects their dependencies.
- Provides lifecycle management for objects (e.g., Singleton, Scoped, Transient).

**Bindings** refer to the configuration that links a key type (interface or base class) to a specific implementation or factory. Methods like `To<T>()`, `To(object instance)`, `ToSelf()`, and `ToFactory<T>()` allow you to specify how objects are created.

**Lifecycle Management** determines how long an object exists within the container:

- Singleton (AsSingle): A single instance is created once for the container’s entire lifetime.
- Scoped (AsScoped): An object is created once per scope. When using nested containers, the current (child) container is checked first for a binding; if none exists, the parent’s binding is used.
- Transient (AsTransient): A new instance is created each time a dependency is resolved.

## Container Configuration

The container is created using the `DiContainer` class constructor. You can specify a configuration, such as the initial capacity for general and scoped objects:

``` csharp
// Example of configuring DiContainer with a configuration
var config = new DiContainer.Config(capacity: 100, scopeCapacity: 5);
var container = new DiContainer(config);
```

In this example, initial capacities are set for general and scoped objects.

## Creating Bindings

After creating the container, you need to register dependencies. This is done using the `Bind<T>()` or `Bind(Type type)` methods, followed by a chain of calls to configure object creation:

``` csharp
// Registering a dependency with an implementation
container.Bind<IMyService>().To<MyService>();

// Registering a dependency for the type itself (ToSelf)
container.Bind<MyService>().ToSelf();

// Registering a specific instance
var instance = new MyService();
container.Bind<IMyService>().To(instance);

// Registering a dependency via a factory
container.Bind<IMyService>().ToFactory<MyServiceFactory>();
```

### Nested Containers

Each such registration binds an interface or type to an implementation that will be used when resolving dependencies.

Suppose we have a parent container with a set of bindings. When creating a child container, you can override some bindings. During dependency resolution, the current (child) container is checked first, and only then the parent container.

``` csharp
// Create a parent container and register a binding
var parentContainer = new DiContainer();
parentContainer.Bind<IMyService>().To<MyService>();

// Create a child container that can use the parent’s binding but has the option to override it
var childContainer = new DiContainer(parentContainer);
childContainer.Bind<IMyService>().To<MyServiceAlternative>();

// Resolving in the parent container returns an instance of MyService:
IMyService parentService = parentContainer.Resolve<IMyService>();

// Resolving in the child container first checks childContainer and finds MyServiceAlternative:
IMyService childService = childContainer.Resolve<IMyService>();

// Thus, bindings in the child container take precedence over those in the parent.
```

### Types with Multiple Constructors

When registering types with multiple constructors, the container uses a specific mechanism to select which constructor will be invoked during instance creation:

- **Single Constructor**: If the type has only one constructor, it is automatically used to create the object.
- **Multiple Constructors**: If there are multiple constructors, the container follows this algorithm:
  - Attribute `[Inject]`: If one constructor is marked with the `[Inject]` attribute, it is selected as the primary constructor for instantiation.
  - Multiple `[Inject]` Attributes: If more than one constructor is marked with `[Inject]`, the system throws an exception, as it cannot determine the priority constructor unambiguously.
  - No `[Inject]`: If no constructor is marked with the attribute, the first constructor from the list of available ones is chosen.

This approach allows explicit specification of which constructor to use (via `[Inject]`), which is particularly useful when a class has constructors with different parameter sets or levels of dependency injection complexity. It helps avoid ambiguity during dependency resolution and ensures correct object creation within the container.

## Dependency Resolution

Once dependencies are registered, the container can create objects and inject their dependencies via constructors. Example of resolving a dependency:

``` csharp
// Resolving a dependency using the generic Resolve<T>() method
IMyService service = container.Resolve<IMyService>();

// Resolving using a type
object serviceObj = container.Resolve(typeof(IMyService));
```

When `Resolve` is called, the container looks for a registered binding. If the type is a collection (e.g., `IEnumerable<T>` or `IReadOnlyList<T>`), the container gathers all registered implementations.

## Lifecycle Management

The container allows you to define the lifespan of objects. Let’s explore the main options:

### Transient (AsTransient)

The `AsTransient()` method ensures a new instance is created each time a dependency is resolved.

``` csharp
container.Bind<IMyService>().To<MyService>().AsTransient();
```

### Singleton (AsSingle)

The `AsSingle()` method guarantees that only one instance is created for all requests:

``` csharp
container.Bind<IMyService>().To<MyService>().AsSingle();
```

### Scoped (AsScoped)

The `AsScoped()` method creates a new instance for each scope of the container. This is particularly important when working with nested containers.

If a binding isn’t registered in the child container, it can use the binding from the parent (via the provided `IBindingProvider`), but the result will be cached locally. This means that even if the parent container has a dependency registered as `Scoped`, resolving it in the child container will create a new instance cached in the child container.

Example of working with `AsScoped` and nested containers:

``` csharp
// Create a parent container and register a dependency with Scoped mode
var parentContainer = new DiContainer();
parentContainer.Bind<IScopedService>().To<ScopedService>().AsScoped();

// Create a child container using the parent as a binding provider
var childContainer = new DiContainer(bindingProvider: parentContainer);

// Resolve the dependency in the parent container
var serviceFromParent = parentContainer.Resolve<IScopedService>();

// Resolve the dependency in the child container
// Although the binding is taken from the parent, caching occurs separately
var serviceFromChild = childContainer.Resolve<IScopedService>();

// serviceFromParent and serviceFromChild will be different instances
```

This approach isolates scopes and prevents unintended state sharing between different parts of the application.

## Recommendations

- **Plan Object Lifetimes**: Use `AsSingle` for services that should maintain a single state across the application, `AsScoped` for context-dependent instances, and `AsTransient` for lightweight or temporary objects.
- **Avoid Binding Conflicts**: When using multiple containers, remember that the child container checks its bindings first. This allows local dependency overrides but requires careful registration management.
- **Proper Resource Disposal**: `DiContainer` implements `IDisposable`, so resources should be properly released when the container is no longer needed.
- **Centralized Registration**: Register all dependencies in one place to simplify maintenance and testing.
- **Minimize Duplication**: Avoid duplicate registrations and carefully monitor conflicts when using multiple containers or binding providers.
