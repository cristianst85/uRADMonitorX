using System;
using NUnit.Framework;

namespace uRADMonitorX.Tests {

    [TestFixture]
    public class ProgramArgumentsTests {

        [TestCase(null, false, false)]
        [TestCase("--allow-multiple-instances", true, false)]
        [TestCase("--allow-multiple-instances --ignore-registering-at-windows-startup", true, true)]
        [TestCase("--ignore-registering-at-windows-startup --allow-multiple-instances", true, true)]
        [TestCase("--ignore-registering-at-windows-startup", false, true)]
        public void Parse(String arguments, bool allowMultipleInstances, bool ignoreRegisteringAtWindowsStartup) {
            ProgramArguments programArguments = null;
            programArguments = ProgramArguments.Parse(arguments != null ? arguments.Split(' ') : new string[] { });
            Assert.AreEqual(allowMultipleInstances, programArguments.AllowMultipleInstances);
            Assert.AreEqual(ignoreRegisteringAtWindowsStartup, programArguments.IgnoreRegisteringAtWindowsStartup);
        }

        [TestCase(null, false, false)]
        [TestCase("--allow-multiple-instances", true, false)]
        [TestCase("--allow-multiple-instances --ignore-registering-at-windows-startup", true, true)]
        [TestCase("--ignore-registering-at-windows-startup --allow-multiple-instances", true, true)]
        [TestCase("--ignore-registering-at-windows-startup", false, true)]
        public void TryParse(String arguments, bool allowMultipleInstances, bool ignoreRegisteringAtWindowsStartup) {
            ProgramArguments programArguments = null;
            bool success = ProgramArguments.TryParse(arguments != null ? arguments.Split(' ') : new string[] { }, out programArguments);
            Assert.IsTrue(success);
            Assert.AreEqual(allowMultipleInstances, programArguments.AllowMultipleInstances);
            Assert.AreEqual(ignoreRegisteringAtWindowsStartup, programArguments.IgnoreRegisteringAtWindowsStartup);
        }

        [Test]
        public void InstanceDefaultValues() {
            ProgramArguments programArguments = new ProgramArguments();
            Assert.IsFalse(programArguments.AllowMultipleInstances);
            Assert.IsFalse(programArguments.IgnoreRegisteringAtWindowsStartup);
        }
    }
}
