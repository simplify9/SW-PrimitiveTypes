using System.Threading;
using System.Threading.Tasks;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Reads the rules a bucket uses to delete files on its own, by key prefix. Registered by the
    /// cloud files providers next to <see cref="ICloudFilesService"/>.
    /// </summary>
    public interface ICloudFilesLifecycle
    {
        /// <summary>
        /// The bucket's deletion rules as the storage service reports them right now. Throws when the
        /// service can't be reached or refuses; sets <see cref="CloudFilesLifecycle.Unavailable"/>
        /// when the provider can't know without more configuration.
        /// </summary>
        Task<CloudFilesLifecycle> GetLifecycleAsync(CancellationToken cancellationToken = default);
    }
}
