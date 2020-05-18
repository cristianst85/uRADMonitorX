using System;
using System.Collections.Generic;

namespace uRADMonitorX
{
    public class ProgramArguments
    {
        [ArgumentDescription("--allow-multiple-instances", "Allows multiple instances to run simultaneously.")]
        public bool AllowMultipleInstances { get; set; }

        [ArgumentDescription("--ignore-registering-at-windows-startup", "Ignores the value of start_with_windows parameter from configuration file.")]
        public bool IgnoreRegisteringAtWindowsStartup { get; set; }

        [ArgumentDescription("--cleanup-update", "Removes old executable file after application update.")]
        public bool CleanupUpdate { get; set; }

        public ProgramArguments()
        {
        }

        public static ProgramArguments Parse(string[] args)
        {
            bool success = InternalParse(args, out ProgramArguments arguments, out Exception exception);

            if (success)
            {
                return arguments;
            }
            else
            {
                throw exception;
            }
        }

        public static bool TryParse(string[] args, out ProgramArguments arguments)
        {
            return InternalParse(args, out arguments, out Exception exception);
        }

        private static bool InternalParse(string[] args, out ProgramArguments arguments, out Exception exception)
        {
            arguments = null;
            exception = null;

            if (args.Length > 3)
            {
                return false;
            }

            var programArguments = new ProgramArguments();
            var keywords = new List<string>();

            foreach (var arg in args)
            {
                if (keywords.Contains(arg))
                {
                    exception = new Exception(string.Format("Duplicate argument '{0}' found.", arg));

                    // Duplicate argument.
                    return false;
                }
                else
                {
                    if (arg.Equals("--allow-multiple-instances"))
                    {
                        programArguments.AllowMultipleInstances = true;
                    }
                    else if (arg.Equals("--ignore-registering-at-windows-startup"))
                    {
                        programArguments.IgnoreRegisteringAtWindowsStartup = true;
                    }
                    else if (arg.Equals("--cleanup-update"))
                    {
                        programArguments.CleanupUpdate = true;
                    }
                    else
                    {
                        exception = new Exception(string.Format("Unknown argument '{0}' found.", arg));

                        // Unknown argument.
                        return false;
                    }

                    keywords.Add(arg);
                }
            }

            arguments = programArguments;

            return true;
        }
    }
}
