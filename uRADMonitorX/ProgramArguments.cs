using System;
using System.Collections.Generic;

namespace uRADMonitorX {

    public class ProgramArguments {

        [ArgumentDescription("--allow-multiple-instances", "Allows multiple instances to run simultaneously.")]
        public bool AllowMultipleInstances { get; set; }

        [ArgumentDescription("--ignore-registering-at-windows-startup", "Ignores the value of start_with_windows parameter from configuration file.")]
        public bool IgnoreRegisteringAtWindowsStartup { get; set; }

        public ProgramArguments() {
        }

        public static ProgramArguments Parse(string[] args) {
            ProgramArguments arguments;
            Exception exception;
            bool success = internalParse(args, out arguments, out exception);
            if (success) {
                return arguments;
            }
            else {
                throw exception;
            }
        }

        public static bool TryParse(string[] args, out ProgramArguments arguments) {
            Exception exception = null;
            return internalParse(args, out arguments, out exception);
        }

        private static bool internalParse(string[] args, out ProgramArguments arguments, out Exception exception) {
            arguments = null;
            exception = null;

            if (args.Length > 2) {
                return false;
            }
            else {
                ProgramArguments programArguments = new ProgramArguments();
                IList<String> keywords = new List<String>();
                foreach (String arg in args) {
                    if (keywords.Contains(arg)) {
                        exception = new Exception(String.Format("Duplicate argument '{0}' found.", arg));
                        return false; // Duplicate argument.
                    }
                    else {
                        if (arg.Equals("--allow-multiple-instances")) {
                            programArguments.AllowMultipleInstances = true;
                        }
                        else if (arg.Equals("--ignore-registering-at-windows-startup")) {
                            programArguments.IgnoreRegisteringAtWindowsStartup = true;
                        }
                        else {
                            exception = new Exception(String.Format("Unknown argument '{0}' found.", arg));
                            return false; // Unknown argument.
                        }
                        keywords.Add(arg);
                    }
                }
                arguments = programArguments;
                return true;
            }
        }
    }
}
