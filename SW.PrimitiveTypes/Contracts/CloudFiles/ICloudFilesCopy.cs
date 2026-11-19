using System.Threading;
using System.Threading.Tasks;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Copies objects inside the bucket on the storage service itself: no bytes pass through the
    /// application. Registered by the cloud files providers next to <see cref="ICloudFilesService"/>;
    /// inject it where a copy or move is needed, so a provider without it fails at startup rather
    /// than halfway through a job.
    /// </summary>
    public interface ICloudFilesCopy
    {
        /// <summary>
        /// Copies <paramref name="sourceKey"/> to <paramref name="destinationKey"/>, replacing any object
        /// already there. Content type and user metadata are copied with the content.
        /// </summary>
        /// <remarks>
        /// Visibility is not copied everywhere: on S3 the copy is private, whatever the source's ACL.
        /// A move is a copy followed by <see cref="ICloudFilesService.DeleteAsync"/>; it is never atomic.
        /// A missing source throws, as <see cref="ICloudFilesService.OpenReadAsync"/> does.
        /// </remarks>
        /// <param name="sourceKey">The object to copy.</param>
        /// <param name="destinationKey">Where to copy it, in the same bucket.</param>
        /// <param name="cancellationToken">Cancels waiting for the copy; a copy the store has started may still finish.</param>
        Task CopyAsync(string sourceKey, string destinationKey, CancellationToken cancellationToken = default);
    }
}
