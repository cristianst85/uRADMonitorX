using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace uRADMonitorX.Commons
{
    public static class AssemblyUtils
    {
        public static string GetApplicationPath()
        {
            return new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        }

        public static string GetApplicationDirPath()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);
        }

        public static Version GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static Version GetVersion(string assemblyName)
        {
            return Assembly.Load(assemblyName).GetName().Version;
        }

        public static string GetProductVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        }

        public static Assembly AssemblyResolver(object sender, ResolveEventArgs args)
        {
            var resourceName = new AssemblyName(args.Name).Name + ".dll";
            var resource = Array.Find(Assembly.GetExecutingAssembly().GetManifestResourceNames(), element => element.EndsWith(resourceName));

            if (resource != null)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    var assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);

                    return Assembly.Load(assemblyData);
                }
            }

            return null;
        }
    }
}
