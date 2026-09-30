# SW.PrimitiveTypes

[![Build Status](https://dev.azure.com/simplify9/Github%20Pipelines/_apis/build/status/simplify9.PrimitiveTypes?branchName=master)](https://dev.azure.com/simplify9/Github%20Pipelines/_build/latest?definitionId=YOUR_DEFINITION_ID&branchName=master)
[![NuGet Version](https://img.shields.io/nuget/v/SimplyWorks.PrimitiveTypes?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SimplyWorks.PrimitiveTypes/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SimplyWorks.PrimitiveTypes?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SimplyWorks.PrimitiveTypes/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

A comprehensive .NET 10 library providing foundational building blocks for modern application development. Essential types, interfaces, and patterns for domain-driven design, value objects, messaging, and API development.

## 🚀 Quick Start

```bash
dotnet add package SimplyWorks.PrimitiveTypes
```

## ✨ Key Features

- 🏗️ **Domain Patterns** - Entities, aggregates, specifications, auditing
- 💎 **Value Objects** - `Money`, `Weight`, `Dimensions`, `StreetAddress` 
- 🚌 **Messaging** - Bus contracts for pub/sub patterns
- 🎯 **API Handlers** - CQRS interfaces for commands and queries
- 🔧 **Extensions** - Type conversion and utility methods
- ☁️ **Cloud Storage** - File operation abstractions

## � Usage Example

```csharp
// Value Objects
var price = new Money(99.99m, "USD");
var weight = new Weight(2.5m, WeightUnit.kg);
var dimensions = new Dimensions(100, 50, 30, DimensionUnit.cm);

// Domain Entity
public class Product : IAggregateRoot
{
    public int Id { get; set; }
    public Money Price { get; set; }
    public Weight ShippingWeight { get; set; }
}

// API Handler
public class GetProductHandler : IGetHandler<int, Product>
{
    public async Task<Product> Handle(int id) => /* implementation */;
}
```

## 📚 Complete Documentation

**📖 [View Full Documentation on GitHub](https://github.com/simplify9/SW-PrimitiveTypes)**

For comprehensive guides, examples, and API reference:
- 🏗️ Architecture patterns and best practices  
- 📝 Detailed code examples and tutorials
- 🔧 Advanced configuration and usage scenarios
- 🤝 Contributing guidelines and development setup


## 🏛️ Related Packages

Part of the Simplify9 ecosystem:
- **[SimplyWorks.Bus](https://github.com/simplify9/Bus)** - Message bus implementation
- **[SimplyWorks.CqApi](https://github.com/simplify9/CqApi)** - Dynamic API generation  
- **[SimplyWorks.Searchy](https://github.com/simplify9/Searchy)** - Search abstraction
- **[SimplyWorks.CloudFiles](https://github.com/simplify9/CloudFiles)** - Cloud storage

## 📄 License

MIT License - see [LICENSE](https://github.com/simplify9/SW-PrimitiveTypes/blob/main/LICENSE) for details.

## 🆘 Support & Contributing

- 🐛 **Issues**: [Report bugs](https://github.com/simplify9/SW-PrimitiveTypes/issues)
- 💬 **Discussions**: [Ask questions](https://github.com/simplify9/SW-PrimitiveTypes/discussions)  
- 🤝 **Contributing**: [Contribution guide](https://github.com/simplify9/SW-PrimitiveTypes#contributing)

---
**Made with ❤️ by [Simplify9](https://github.com/simplify9)** | **[Full Documentation →](https://github.com/simplify9/SW-PrimitiveTypes)**
