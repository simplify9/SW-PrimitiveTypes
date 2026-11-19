using System;

namespace SW.PrimitiveTypes
{
    /// <summary>
    /// Connection settings shared by the storage providers, bound from the <c>CloudFiles</c>
    /// configuration section. Each provider adds its own options in a subclass.
    /// </summary>
    public class CloudFilesOptions
    {

        public const string ConfigurationSection = "CloudFiles";

        /// <summary>Access key (S3) or account name (Azure shared key).</summary>
        public string AccessKeyId { get; set; }
        /// <summary>Secret key (S3) or account key (Azure shared key).</summary>
        public string SecretAccessKey { get; set; }
        /// <summary>The bucket, or Azure container, every key is relative to.</summary>
        public string BucketName { get; set; }
        /// <summary>The store's endpoint, such as an S3-compatible endpoint or an Azure blob service URL.</summary>
        public string ServiceUrl { get; set; }
        /// <summary>
        /// Not read by the SimplyWorks.CloudFiles providers: the provider is the one whose
        /// <c>Add*CloudFiles()</c> the application calls. Applications may use it to choose one.
        /// </summary>
        public string Driver { get; set; } = "S3";
    }
}
