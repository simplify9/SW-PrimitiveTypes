using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Names the contract a type's shape is published as: a stable key and a major version that changes
    /// only on a breaking change. It's a label and does nothing by itself; libraries that move the type
    /// (a bus publisher, an API host, an adapter runner) and schema tools read it.
    /// </summary>
    /// <remarks>
    /// Not inherited: a derived type has a shape of its own, so it names its own contract.
    /// The constructor rejects only a missing key, because an attribute constructor that throws breaks
    /// every caller that reflects over the type. Use <see cref="IsWellFormed"/> to check the rest.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class ContractAttribute : Attribute
    {
        static readonly Regex KeyFormat = new Regex("^[a-z0-9]+(\\.[a-z0-9]+)*$", RegexOptions.CultureInvariant);

        public ContractAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        /// <summary>Lowercase, dot-separated: "orders.msg.placed".</summary>
        public string Key { get; }

        /// <summary>Changes only when the shape changes in a way old readers can't handle.</summary>
        public int Major { get; set; } = 1;

        /// <summary>"orders.msg.placed@1".</summary>
        public string Id => $"{Key}@{Major}";

        /// <summary>The key is lowercase and dot-separated, and the major is at least 1.</summary>
        public bool IsWellFormed => KeyFormat.IsMatch(Key) && Major >= 1;

        /// <summary>The contract declared on the type itself, or null.</summary>
        public static ContractAttribute Of(Type type) =>
            type?.GetCustomAttribute<ContractAttribute>(inherit: false);

        public override string ToString() => Id;
    }
}
