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
- 🏷️ **Contract labels** - `[Contract("orders.msg.placed", Major = 2)]` names the published shape of a type and its major version, for libraries and schema tools to read

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


## ☁️ Cloud files contract

`SW.PrimitiveTypes` defines the object-storage contract that the
[SimplyWorks.CloudFiles](https://github.com/simplify9/SW-CloudFiles) providers implement
(S3 and S3-compatible stores, Azure Blob, Google Cloud Storage, Oracle Object Storage, and a local store for tests):

| Type | Role |
|------|------|
| `ICloudFilesService` | write, read, list, metadata, delete, public and signed URLs, streamed upload (S3 only) |
| `ICloudFilesLifecycle` | reads the bucket's age-based deletion rules (`temp1/`, `temp7/`, `temp30/`, `temp365/` and any others) |
| `ICloudFilesCopy` | server-side copy inside the bucket (a move is copy + delete) |
| `ICloudFilesProperties` | one object's size, content type, ETag, last-modified and metadata without downloading it, or `null` when it doesn't exist; `ExistsAsync` |
| `WriteFileSettings`, `CloudFileInfo`, `RemoteBlob`, `WriteWrapper` | inputs and results |
| `CloudFilesOptions` | connection settings, bound from the `CloudFiles` configuration section |

The members are the same on every provider, but some behaviour differs: metadata keys, the result of
deleting a missing key, which exception a missing key throws, and URL shapes. The XML docs note the main
differences; the full table is the CloudFiles
[provider behaviour reference](https://github.com/simplify9/SW-CloudFiles/blob/main/docs/provider-behavior.md).

**Changing the contract.** Applications implement `ICloudFilesService` too (test fakes, stubs,
decorators), so a new member breaks them even though it looks additive. Never add, change or remove
members of `ICloudFilesService` or the shared models. Add a capability as a new interface that providers
implement and register in DI beside `ICloudFilesService`, as `ICloudFilesLifecycle` is; add new model
fields as optional properties. Release PrimitiveTypes first, then the providers, then consumers. Rules,
roadmap and what stays out of scope:
[Evolving the contract](https://github.com/simplify9/SW-CloudFiles/blob/main/docs/evolving-the-contract.md).

## 🏛️ Related Packages

Part of the Simplify9 ecosystem:
- **[SimplyWorks.Bus](https://github.com/simplify9/Bus)** - Message bus implementation
- **[SimplyWorks.CqApi](https://github.com/simplify9/CqApi)** - Dynamic API generation  
- **[SimplyWorks.Searchy](https://github.com/simplify9/Searchy)** - Search abstraction
- **[SimplyWorks.CloudFiles](https://github.com/simplify9/SW-CloudFiles)** - Cloud storage

## 📄 License

MIT License - see [LICENSE](https://github.com/simplify9/SW-PrimitiveTypes/blob/main/LICENSE) for details.

## 🆘 Support & Contributing

- 🐛 **Issues**: [Report bugs](https://github.com/simplify9/SW-PrimitiveTypes/issues)
- 💬 **Discussions**: [Ask questions](https://github.com/simplify9/SW-PrimitiveTypes/discussions)  
- 🤝 **Contributing**: [Contribution guide](https://github.com/simplify9/SW-PrimitiveTypes#contributing)

---
**Made with ❤️ by [Simplify9](https://github.com/simplify9)** | **[Full Documentation →](https://github.com/simplify9/SW-PrimitiveTypes)**
