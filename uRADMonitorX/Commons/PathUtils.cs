using System;
using System.IO;

namespace uRADMonitorX.Commons {

    public static class PathUtils {

        public static String GetFullPath(String applicationDirPath, String relativeOrAbsolutePath) {
            if (String.IsNullOrEmpty(relativeOrAbsolutePath) || relativeOrAbsolutePath.StartsWith(Path.DirectorySeparatorChar.ToString())) {
                return Path.Combine(applicationDirPath, relativeOrAbsolutePath.TrimStart(Path.DirectorySeparatorChar));
            }
            else {
                return relativeOrAbsolutePath;
            }
        }
    }
}
