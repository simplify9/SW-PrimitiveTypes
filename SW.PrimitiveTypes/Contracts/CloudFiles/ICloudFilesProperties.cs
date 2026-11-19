using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Reads one object's properties without downloading it, and tells whether it exists. Registered
    /// by the cloud files providers next to <see cref="ICloudFilesService"/>.
    /// </summary>
    public interface ICloudFilesProperties
    {
        /// <summary>
        /// The properties of <paramref name="key"/>, or <c>null</c> when no object has exactly that key.
        /// One request (a HEAD on most stores), whatever the object's size.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="ICloudFilesService.ListAsync"/>, which matches by prefix, this looks up the
        /// exact key. Unlike <see cref="ICloudFilesService.GetMetadataAsync"/>, a missing key is
        /// <c>null</c> on every provider instead of a provider-specific exception.
        /// </remarks>
        /// <param name="key">The object's key.</param>
        /// <param name="cancellationToken">Cancels the request.</param>
        Task<CloudFileProperties> GetPropertiesAsync(string key, CancellationToken cancellationToken = default);
    }

    /// <summary>One object's properties, read by <see cref="ICloudFilesProperties.GetPropertiesAsync"/>.</summary>
    public class CloudFileProperties
    {
        /// <summary>The object's key.</summary>
        public string Key { get; set; }

        /// <summary>Size in bytes.</summary>
        public long ContentLength { get; set; }

        /// <summary>The stored content type, or <c>null</c> when the store has none.</summary>
        public string ContentType { get; set; }

        /// <summary>
        /// An opaque change marker with its quotes removed (the ETag on most stores, the generation-based
        /// ETag on Google). Compare it for equality only; it is not always a checksum of the content.
        /// </summary>
        public string ETag { get; set; }

        /// <summary>When the object was last written, where the store reports it.</summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>User metadata, with case-insensitive keys (S3 lowercases them on write).</summary>
        public IReadOnlyDictionary<string, string> Metadata { get; set; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Shortcuts over <see cref="ICloudFilesProperties"/>.</summary>
    public static class CloudFilesPropertiesExtensions
    {
        /// <summary>Whether an object with exactly <paramref name="key"/> exists.</summary>
        public static async Task<bool> ExistsAsync(this ICloudFilesProperties properties, string key,
            CancellationToken cancellationToken = default) =>
            await properties.GetPropertiesAsync(key, cancellationToken) != null;
    }
}
