# SW.PrimitiveTypes

**Foundational .NET library with essential types, patterns, and interfaces for modern application development.**

## Quick Start

```bash
dotnet add package SimplyWorks.PrimitiveTypes
```

## What's Included

### 💎 Value Objects
Ready-to-use business types with built-in validation and conversion:
- `Money` - Currency handling with conversion support
- `Weight` - Mass measurements (kg, lb, oz, g)
- `Dimensions` - 3D measurements with unit conversion
- `StreetAddress` - Structured address representation

### 🏗️ Domain Patterns  
Clean architecture building blocks:
- `IEntity<T>` - Entity interfaces with identity
- `IAggregateRoot<T>` - Domain aggregate markers
- `ISpecification<T>` - Business rule specifications
- `ValueObject` - Base class with equality semantics

### 🎯 API Handlers
CQRS-style interfaces for clean APIs:
- `IGetHandler<TKey, TResult>` - Query operations
- `ICommandHandler<TRequest, TResult>` - Command operations  
- `IDeleteHandler<TKey>` - Delete operations

### 🚌 Messaging Contracts
Event-driven architecture interfaces:
- `IPublish` / `IConsume<T>` - Message publishing/consumption
- `IBroadcast` / `IListen<T>` - Event broadcasting/listening

## Usage Example

```csharp
// Value Objects
var price = new Money(99.99m, "USD");
var weight = new Weight(2.5m, WeightUnit.kg);
var size = new Dimensions(30, 20, 10, DimensionUnit.cm);

// Domain Entity
public class Product : IAggregateRoot
{
    public int Id { get; set; }
    public Money Price { get; set; }
    public Weight ShippingWeight { get; set; }
    public Dimensions PackageDimensions { get; set; }
}

// API Handler
public class GetProductHandler : IGetHandler<int, Product>
{
    public async Task<Product> Handle(int id)
    {
        // Retrieve and return product
        return await productRepository.GetByIdAsync(id);
    }
}
```

## Related Packages

Part of the **Simplify9** ecosystem:
- [SimplyWorks.Bus](https://www.nuget.org/packages/SimplyWorks.Bus) - Message bus implementation
- [SimplyWorks.CqApi](https://www.nuget.org/packages/SimplyWorks.CqApi) - Dynamic API generation
- [SimplyWorks.Searchy](https://www.nuget.org/packages/SimplyWorks.Searchy) - Search abstractions

## 📚 Complete Documentation

**[📖 Full Documentation & Examples →](https://github.com/simplify9/SW-PrimitiveTypes)**

Visit our GitHub repository for comprehensive guides, advanced examples, and contribution guidelines.

---

**MIT Licensed** | **Made by [Simplify9](https://github.com/simplify9)**