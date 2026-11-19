using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Object storage behind one interface: one bucket (an Azure container), addressed by string keys.
    /// Implemented by the SimplyWorks.CloudFiles providers (S3 and S3-compatible stores, Azure Blob,
    /// Google Cloud Storage, Oracle Object Storage, and a local filesystem store for tests).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The signatures are the same on every provider; the behaviour is not always. Metadata keys,
    /// what <see cref="DeleteAsync"/> returns for a missing key, the exception thrown for a missing
    /// key, URL shapes and the cost of a signed URL all differ. The per-provider reference is
    /// docs/provider-behavior.md in https://github.com/simplify9/SW-CloudFiles.
    /// </para>
    /// <para>
    /// Applications also implement this interface (fakes, stubs, decorators), so adding a member
    /// breaks them. New capabilities are added as separate interfaces registered next to this one,
    /// as <see cref="ICloudFilesLifecycle"/>, <see cref="ICloudFilesCopy"/> and
    /// <see cref="ICloudFilesProperties"/> are. Never add, change or remove members here.
    /// </para>
    /// </remarks>
    public interface ICloudFilesService
    {
        /// <summary>
        /// Uploads <paramref name="inputStream"/> to <see cref="WriteFileSettings.Key"/>, replacing any
        /// object already stored under that key.
        /// </summary>
        /// <param name="inputStream">
        /// The content. Use a seekable stream (memory or file): the S3 and Azure providers read its
        /// <see cref="Stream.Length"/> and throw for streams that can't report one.
        /// </param>
        /// <param name="settings">
        /// Key, content type, metadata and visibility. <see cref="WriteFileSettings.Public"/> and
        /// <see cref="WriteFileSettings.Metadata"/> are not honoured by every provider.
        /// </param>
        /// <returns>
        /// The stored blob. <see cref="RemoteBlob.Location"/> may be a signed URL that expires after an
        /// hour, so store the key, not the location.
        /// </returns>
        Task<RemoteBlob> WriteAsync(Stream inputStream, WriteFileSettings settings);

        /// <summary>
        /// Uploads <paramref name="text"/> as UTF-8 to <see cref="WriteFileSettings.Key"/>, replacing any
        /// object already stored under that key. The content type defaults to <c>text/plain</c>.
        /// </summary>
        /// <param name="text">The content.</param>
        /// <param name="settings">
        /// As for <see cref="WriteAsync"/>; the S3 provider ignores
        /// <see cref="WriteFileSettings.Metadata"/> here.
        /// </param>
        /// <returns>The stored blob; see <see cref="WriteAsync"/>.</returns>
        Task<RemoteBlob> WriteTextAsync(string text, WriteFileSettings settings);

        /// <summary>
        /// A read-only URL for <paramref name="key"/> that works whether or not the object is public,
        /// until <paramref name="expiry"/> has passed.
        /// </summary>
        /// <remarks>
        /// Computed locally on S3, Google and Azure with a shared key. Azure with a managed identity makes
        /// a network call per URL, and Oracle creates a server-side pre-authenticated request per URL,
        /// so don't generate them in loops on those providers.
        /// </remarks>
        /// <param name="key">The object's key.</param>
        /// <param name="expiry">How long the URL stays valid, from now.</param>
        string GetSignedUrl(string key, TimeSpan expiry);

        /// <summary>
        /// The permanent, unsigned URL of <paramref name="key"/>. It is built without calling the store
        /// and works only when the object or its bucket is publicly readable.
        /// </summary>
        /// <param name="key">The object's key.</param>
        string GetUrl(string key);

        /// <summary>
        /// Starts a streamed upload to <see cref="WriteFileSettings.Key"/> through a single HTTP PUT:
        /// write to <see cref="WriteWrapper.GetStreamAsync"/>, then call
        /// <see cref="WriteWrapper.CompleteRequestAsync"/>.
        /// </summary>
        /// <remarks>
        /// S3 only. Other providers throw <see cref="NotImplementedException"/> or don't work. Prefer
        /// <see cref="WriteAsync"/>.
        /// </remarks>
        /// <param name="settings">Key, content type and visibility of the object to write.</param>
        WriteWrapper OpenWrite(WriteFileSettings settings);

        /// <summary>
        /// Opens <paramref name="key"/> for reading. The caller must dispose the stream.
        /// </summary>
        /// <remarks>
        /// Most providers stream from the network; the Google provider buffers the whole object in
        /// memory first. A missing key throws, and the exception type is the provider SDK's own
        /// (a 404 status in every network provider, <see cref="FileNotFoundException"/> locally).
        /// </remarks>
        /// <param name="key">The object's key.</param>
        Task<Stream> OpenReadAsync(string key);

        /// <summary>
        /// Every object whose key starts with <paramref name="prefix"/>, including objects in nested
        /// "folders". The whole listing is returned, one request per 1,000 keys, and held in memory.
        /// </summary>
        /// <remarks>
        /// The S3 and Oracle providers returned only the first 1,000 keys before SimplyWorks.CloudFiles
        /// 10.0.4. The match is by prefix: to test whether one key exists, use
        /// <see cref="ICloudFilesProperties"/>, or compare <see cref="CloudFileInfo.Key"/> exactly instead of
        /// calling <c>Any()</c> on the result.
        /// </remarks>
        /// <param name="prefix">Key prefix, such as <c>"invoices/2026/"</c>. Empty lists the bucket.</param>
        Task<IEnumerable<CloudFileInfo>> ListAsync(string prefix);

        /// <summary>
        /// The user metadata stored with <paramref name="key"/>, plus some system values on some
        /// providers.
        /// </summary>
        /// <remarks>
        /// Only user metadata is returned everywhere. <c>Hash</c>, <c>ContentType</c> and
        /// <c>ContentLength</c> are added by S3 and Azure; Oracle adds <c>Hash</c> and
        /// <c>ContentType</c>; Google adds none. Read keys case-insensitively: S3 lowercases them.
        /// A missing key throws, as for <see cref="OpenReadAsync"/>.
        /// </remarks>
        /// <param name="key">The object's key.</param>
        Task<IReadOnlyDictionary<string, string>> GetMetadataAsync(string key);

        /// <summary>
        /// Deletes <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// The result for a key that doesn't exist depends on the provider: S3 and local return
        /// <c>true</c>, Google and Oracle return <c>false</c>, Azure throws. Oracle also returns
        /// <c>false</c> for any other failure. Check first if you need to know whether it existed.
        /// </remarks>
        /// <param name="key">The object's key.</param>
        /// <returns><c>true</c> when the store accepted the delete; see remarks.</returns>
        Task<bool> DeleteAsync(string key);
    }
}
