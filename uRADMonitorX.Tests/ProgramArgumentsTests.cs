using System;
using NUnit.Framework;

namespace uRADMonitorX.Tests {

    [TestFixture]
    public class ProgramArgumentsTests {

        [TestCase(null, false, false, false)]
        [TestCase("--allow-multiple-instances", true, false, false)]
        [TestCase("--allow-multiple-instances --ignore-registering-at-windows-startup", true, true, false)]
        [TestCase("--ignore-registering-at-windows-startup --allow-multiple-instances", true, true, false)]
        [TestCase("--ignore-registering-at-windows-startup", false, true, false)]
        [TestCase("--cleanup-update", false, false, true)]
        [TestCase("--allow-multiple-instances --cleanup-update", true, false, true)]
        public void Parse(String arguments, bool allowMultipleInstances, bool ignoreRegisteringAtWindowsStartup, bool cleanupUpdate) {
            ProgramArguments programArguments = null;
            programArguments = ProgramArguments.Parse(arguments != null ? arguments.Split(' ') : new string[] { });
            Assert.AreEqual(allowMultipleInstances, programArguments.AllowMultipleInstances);
            Assert.AreEqual(ignoreRegisteringAtWindowsStartup, programArguments.IgnoreRegisteringAtWindowsStartup);
            Assert.AreEqual(cleanupUpdate, programArguments.CleanupUpdate);
        }

        [TestCase(null, false, false, false)]
        [TestCase("--allow-multiple-instances", true, false, false)]
        [TestCase("--allow-multiple-instances --ignore-registering-at-windows-startup", true, true, false)]
        [TestCase("--ignore-registering-at-windows-startup --allow-multiple-instances", true, true, false)]
        [TestCase("--ignore-registering-at-windows-startup", false, true, false)]
        [TestCase("--cleanup-update", false, false, true)]
        [TestCase("--allow-multiple-instances --cleanup-update", true, false, true)]
        public void TryParse(String arguments, bool allowMultipleInstances, bool ignoreRegisteringAtWindowsStartup, bool cleanupUpdate) {
            ProgramArguments programArguments = null;
            bool success = ProgramArguments.TryParse(arguments != null ? arguments.Split(' ') : new string[] { }, out programArguments);
            Assert.IsTrue(success);
            Assert.AreEqual(allowMultipleInstances, programArguments.AllowMultipleInstances);
            Assert.AreEqual(ignoreRegisteringAtWindowsStartup, programArguments.IgnoreRegisteringAtWindowsStartup);
            Assert.AreEqual(cleanupUpdate, programArguments.CleanupUpdate);
        }

        [Test]
        public void InstanceDefaultValues() {
            ProgramArguments programArguments = new ProgramArguments();
            Assert.IsFalse(programArguments.AllowMultipleInstances);
            Assert.IsFalse(programArguments.IgnoreRegisteringAtWindowsStartup);
            Assert.IsFalse(programArguments.CleanupUpdate);
        }
    }
}
