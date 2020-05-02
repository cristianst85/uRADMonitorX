using NUnit.Framework;
using System;
using uRADMonitorX.Commons.Formatting;

namespace uRADMonitorX.Tests.Commons.Formatting
{
    [TestFixture]
    public class TimeSpanFormatterTest
    {
        [TestCase(0, "0s")]
        [TestCase(25, "25s")]
        [TestCase(59, "59s")]
        [TestCase(60, "1m 0s")]
        [TestCase(120, "2m 0s")]
        [TestCase(150, "2m 30s")]
        [TestCase(3599, "59m 59s")]
        [TestCase(3600, "1h 0m 0s")]
        [TestCase(86399, "23h 59m 59s")]
        [TestCase(86400, "1d 0h 0m 0s")]
        [TestCase(90061, "1d 1h 1m 1s")]
        public void Format(int seconds, string expectedString)
        {
            var timeSpanFormatter = new TimeSpanFormatter();
            Assert.AreEqual(expectedString, timeSpanFormatter.Format(TimeSpan.FromSeconds(seconds)));
        }
    }
}
