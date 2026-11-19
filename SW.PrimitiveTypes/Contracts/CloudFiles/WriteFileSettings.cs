using System;
using System.Collections.Generic;
using System.Text;

namespace SW.PrimitiveTypes
{ 
    /// <summary>What to write and how: passed to <see cref="ICloudFilesService.WriteAsync"/>,
    /// <see cref="ICloudFilesService.WriteTextAsync"/> and <see cref="ICloudFilesService.OpenWrite"/>.</summary>
    public class WriteFileSettings
    {
        public WriteFileSettings()
        {
            Metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }


        /// <summary>
        /// The object's key inside the bucket: path and file name, separated by <c>/</c>, with no leading
        /// slash (<c>"invoices/2026/INV-1.pdf"</c>). Keys under <c>temp1/</c>, <c>temp7/</c>, <c>temp30/</c>
        /// and <c>temp365/</c> are deleted by the store after that many days where the provider set up
        /// its lifecycle rules (see <see cref="ICloudFilesLifecycle"/>).
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The MIME type stored with the object. When null, most providers store
        /// <c>application/octet-stream</c> (<c>text/plain</c> for text writes).
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Asks for the object to be publicly readable. Only the S3 provider acts on it, by sending the
        /// <c>public-read</c> canned ACL, and stores without object ACLs ignore that. Azure, Google and
        /// Oracle decide visibility per bucket or container. Don't rely on it for privacy either way:
        /// configure the bucket.
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// Close the input stream once uploaded (default <c>true</c>). Only the S3 provider honours it;
        /// the others leave the stream open, so dispose it yourself.
        /// </summary>
        public bool CloseInputStream { get; set; } = true;

        /// <summary>
        /// User metadata to store with the object, read back by
        /// <see cref="ICloudFilesService.GetMetadataAsync"/>. Ignored by the Google provider, and by the S3
        /// provider on text writes. Use lowercase ASCII keys: S3 lowercases them.
        /// </summary>
        public IDictionary<string, string> Metadata { get; set; }
    }
}
