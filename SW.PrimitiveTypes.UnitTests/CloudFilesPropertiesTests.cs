using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SW.PrimitiveTypes.UnitTests
{
    [TestClass]
    public class CloudFilesPropertiesTests
    {
        private sealed class OneFile(string key) : ICloudFilesProperties
        {
            public Task<CloudFileProperties> GetPropertiesAsync(string k, CancellationToken cancellationToken = default) =>
                Task.FromResult(k == key ? new CloudFileProperties { Key = k, ContentLength = 3 } : null);
        }

        [TestMethod]
        public async Task Exists_only_for_the_exact_key()
        {
            var store = new OneFile("adapters/foo");

            Assert.IsTrue(await store.ExistsAsync("adapters/foo"));
            Assert.IsFalse(await store.ExistsAsync("adapters/fo"));
            Assert.IsFalse(await store.ExistsAsync("adapters/foo-bar"));
        }

        [TestMethod]
        public void Default_metadata_is_empty_and_reads_keys_case_insensitively()
        {
            var properties = new CloudFileProperties();
            Assert.AreEqual(0, properties.Metadata.Count);

            var metadata = (System.Collections.Generic.IDictionary<string, string>)properties.Metadata;
            metadata["uploaded-by"] = "someone";
            Assert.AreEqual("someone", properties.Metadata["Uploaded-By"]);
        }
    }
}
