using System;
using System.Collections.Generic;
using System.Text;

namespace SW.PrimitiveTypes
{
    /// <summary>The result of writing an object to storage.</summary>
    public class RemoteBlob
    {
        /// <summary>
        /// A URL for the object. Depending on the provider it is the public URL, a signed URL valid for one
        /// hour, or an unsigned URL that a private bucket refuses. Store the key, not this.
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Size in bytes, where the provider reports it (not every provider does, and the S3 provider
        /// reports characters for text writes). An <c>int</c>, so it overflows above 2 GiB.
        /// </summary>
        public int Size { get; set; }
        /// <summary>The content type that was requested for the object.</summary>
        public string MimeType { get; set; }
        /// <summary>The object's key (not set by every provider).</summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
