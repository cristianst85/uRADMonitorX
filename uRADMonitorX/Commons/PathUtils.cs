using System.IO;

namespace uRADMonitorX.Commons
{
    public static class PathUtils
    {
        public static string GetFullPath(string applicationDirPath, string relativeOrAbsolutePath)
        {
            if (string.IsNullOrEmpty(relativeOrAbsolutePath) || relativeOrAbsolutePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return Path.Combine(applicationDirPath, relativeOrAbsolutePath.TrimStart(Path.DirectorySeparatorChar));
            }

            return relativeOrAbsolutePath;
        }
    }
}
