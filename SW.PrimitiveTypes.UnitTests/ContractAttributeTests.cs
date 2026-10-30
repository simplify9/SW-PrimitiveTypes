using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SW.PrimitiveTypes.UnitTests
{
    [TestClass]
    public class ContractAttributeTests
    {
        [Contract("orders.msg.placed", Major = 2)]
        class OrderPlaced { }

        class OrderPlacedWithGift : OrderPlaced { }

        [Contract("orders.document.invoice")]
        struct Invoice { }

        [TestMethod]
        public void Reads_key_and_major()
        {
            var c = ContractAttribute.Of(typeof(OrderPlaced));
            Assert.AreEqual("orders.msg.placed", c.Key);
            Assert.AreEqual(2, c.Major);
            Assert.AreEqual("orders.msg.placed@2", c.Id);
            Assert.AreEqual("orders.msg.placed@2", c.ToString());
        }

        [TestMethod]
        public void Major_defaults_to_1() =>
            Assert.AreEqual("orders.document.invoice@1", ContractAttribute.Of(typeof(Invoice)).Id);

        [TestMethod]
        public void A_derived_type_does_not_take_its_base_contract()
        {
            Assert.IsNull(ContractAttribute.Of(typeof(OrderPlacedWithGift)));
            Assert.IsNull(Attribute.GetCustomAttribute(typeof(OrderPlacedWithGift), typeof(ContractAttribute), inherit: true));
        }

        [TestMethod]
        public void An_unlabelled_type_or_null_has_none()
        {
            Assert.IsNull(ContractAttribute.Of(typeof(string)));
            Assert.IsNull(ContractAttribute.Of(null));
        }

        [DataTestMethod]
        [DataRow("orders.msg.placed", 1, true)]
        [DataRow("orders", 3, true)]
        [DataRow("orders2.v2", 1, true)]
        [DataRow("Orders.msg.placed", 1, false)]
        [DataRow("orders..placed", 1, false)]
        [DataRow("orders.msg.", 1, false)]
        [DataRow("orders-msg", 1, false)]
        [DataRow("orders.msg.placed", 0, false)]
        [DataRow("orders\n", 1, false)]
        [DataRow("orders.msg.placed\n", 1, false)]
        [DataRow(" orders", 1, false)]
        public void Checks_its_form(string key, int major, bool wellFormed) =>
            Assert.AreEqual(wellFormed, new ContractAttribute(key) { Major = major }.IsWellFormed);

        [TestMethod]
        public void A_missing_key_is_refused()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ContractAttribute(null));
            Assert.ThrowsException<ArgumentNullException>(() => new ContractAttribute(" "));
        }
    }
}
