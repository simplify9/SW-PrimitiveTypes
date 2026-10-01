using System;
using System.Collections.Generic;
using System.Linq;

namespace SW.PrimitiveTypes
{
    /// <summary>The rules one bucket (an Azure container) uses to delete files by age.</summary>
    public class CloudFilesLifecycle
    {
        /// <summary>The storage service that answered: S3, Azure, Oracle, Google or Local.</summary>
        public string Provider { get; set; }

        /// <summary>The bucket, or Azure container, the rules apply to.</summary>
        public string Bucket { get; set; }

        /// <summary>
        /// Why the rules couldn't be read, or <c>null</c> when <see cref="Rules"/> is the bucket's real
        /// rule set. No reason and no rules means nothing deletes the files.
        /// </summary>
        public string Unavailable { get; set; }

        /// <summary>
        /// Only rules that delete files a number of days after they were written. Rules that move files
        /// to another storage tier, delete on a fixed date or only touch tagged files are left out.
        /// </summary>
        public IReadOnlyList<CloudFilesLifecycleRule> Rules { get; set; } = Array.Empty<CloudFilesLifecycleRule>();

        /// <summary>
        /// The enabled rule that deletes <paramref name="key"/> soonest, or <c>null</c> when none applies.
        /// Storage services apply the shortest expiration when several rules match one file.
        /// </summary>
        public CloudFilesLifecycleRule RuleFor(string key) =>
            key == null
                ? null
                : Rules
                    .Where(r => r.Enabled && key.StartsWith(r.Prefix ?? string.Empty, StringComparison.Ordinal))
                    .OrderBy(r => r.Days)
                    .FirstOrDefault();
    }

    /// <summary>One rule that deletes files under a prefix after a number of days.</summary>
    public class CloudFilesLifecycleRule
    {
        /// <summary>The rule's name in the storage service, where it has one.</summary>
        public string Id { get; set; }

        /// <summary>The key prefix the rule applies to, relative to the bucket. Empty covers every file.</summary>
        public string Prefix { get; set; }

        /// <summary>Files are deleted this many days after they were last written.</summary>
        public int Days { get; set; }

        /// <summary>A disabled rule is reported but deletes nothing.</summary>
        public bool Enabled { get; set; }
    }
}
