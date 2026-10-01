using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SW.PrimitiveTypes.UnitTests
{
    [TestClass]
    public class CloudFilesLifecycleTests
    {
        private static CloudFilesLifecycle Lifecycle(params CloudFilesLifecycleRule[] rules) =>
            new CloudFilesLifecycle { Provider = "S3", Bucket = "bucket", Rules = rules };

        private static CloudFilesLifecycleRule Rule(string prefix, int days, bool enabled = true) =>
            new CloudFilesLifecycleRule { Id = prefix, Prefix = prefix, Days = days, Enabled = enabled };

        [TestMethod]
        public void Matches_the_rule_whose_prefix_starts_the_key()
        {
            var lifecycle = Lifecycle(Rule("temp7/", 7), Rule("temp30/", 30));

            Assert.AreEqual(30, lifecycle.RuleFor("temp30/docs/abc/input")!.Days);
        }

        [TestMethod]
        public void No_rule_when_no_prefix_matches()
        {
            var lifecycle = Lifecycle(Rule("temp30/", 30));

            Assert.IsNull(lifecycle.RuleFor("archive/docs/abc.json"));
        }

        [TestMethod]
        public void The_shortest_of_several_matching_rules_wins()
        {
            var lifecycle = Lifecycle(Rule("", 365), Rule("temp30/", 30), Rule("temp30/docs/", 90));

            Assert.AreEqual(30, lifecycle.RuleFor("temp30/docs/abc/input")!.Days);
        }

        [TestMethod]
        public void A_disabled_rule_deletes_nothing()
        {
            var lifecycle = Lifecycle(Rule("temp30/", 30, enabled: false));

            Assert.IsNull(lifecycle.RuleFor("temp30/docs/abc/input"));
        }

        [TestMethod]
        public void Prefixes_are_case_sensitive_like_storage_keys()
        {
            var lifecycle = Lifecycle(Rule("temp30/", 30));

            Assert.IsNull(lifecycle.RuleFor("Temp30/docs/abc/input"));
        }
    }
}
