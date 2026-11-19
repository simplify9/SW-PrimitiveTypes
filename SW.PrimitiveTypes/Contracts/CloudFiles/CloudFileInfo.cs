using System;
using System.Collections.Generic;
using System.Text;

namespace SW.PrimitiveTypes
{
    /// <summary>One object returned by <see cref="ICloudFilesService.ListAsync"/>.</summary>
    public class CloudFileInfo
    {
        /// <summary>The object's full key, including the listed prefix.</summary>
        public string Key { get; set; }

        /// <summary>Size in bytes. Zero-byte keys ending in <c>/</c> are folder placeholders.</summary>
        public long Size { get; set; }

        /// <summary>
        /// An opaque change marker: the ETag on S3 and Azure, the base64 MD5 on Google, <c>null</c> on
        /// Oracle and the local store. Compare it for equality only; it is not always a checksum.
        /// </summary>
        public string Signature { get; set; }
    }
}
