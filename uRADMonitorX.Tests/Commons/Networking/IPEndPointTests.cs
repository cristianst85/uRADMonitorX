using System;
using NUnit.Framework;
using uRADMonitorX.Commons.Networking;

namespace uRADMonitorX.Tests.Commons.Networking {

    [TestFixture]
    public class IPEndPointTests {

        [TestCase("", false)]
        [TestCase("abcdef", false)]
        [TestCase("127.0.0.1:80", true)]
        [TestCase("127.0.0.1:8080", true)]
        [TestCase("127.0.0.1:1", true)]
        [TestCase("127.0.0.1:65535", true)]
        [TestCase("127.0.0.1:0", false)]
        [TestCase("127.0.0.1:65536", false)]        
        [TestCase("127.0.0.1", false)]
        [TestCase("127.0.0", false)]
        [TestCase("127.0.0:80", false)]
        [TestCase("127.0.0:", false)]
        public void IsValid(String ipAddressWithPort, bool expectedResult) {
            Assert.AreEqual(expectedResult, IPEndPoint.IsValidFormat(ipAddressWithPort));
        }
    }
}
