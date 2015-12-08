using System;
using System.Reflection;

namespace uRADMonitorX.Commons {

    public static class AssemblyUtils {

        public static String GetApplicationPath() {
            return new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        }

        public static Version GetVersion() {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName assemblyName = assembly.GetName();
            return assemblyName.Version;
        }

        public static Assembly AssemblyResolver(object sender, ResolveEventArgs args) {
            string resourceName = new AssemblyName(args.Name).Name + ".dll";
            string resource = Array.Find(Assembly.GetExecutingAssembly().GetManifestResourceNames(), element => element.EndsWith(resourceName));
            if (resource != null) {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource)) {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }
            return null;
        }
    }
}