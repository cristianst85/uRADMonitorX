using System;
using System.IO;
using System.Security.Cryptography;

namespace uRADMonitorX.Commons {

    public static class ChecksumUtils {
    
        public static String ToHex(byte[] hash) {
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public static String ComputeMD5(String filePath) {
            FileStream fs = null;
            MD5 md5 = null;
            try {
                md5 = MD5.Create();
                md5.Initialize();
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
                md5.ComputeHash(fs);
                return ChecksumUtils.ToHex(md5.Hash).ToLower();
            }
            catch {
                throw;
            }
            finally {
                if (fs != null) {
                    fs.Dispose();
                }
                if (md5 != null) {
                    md5.Clear(); // No dispose method in .NET 2.0 !?
                }
            }
        }

        public static String ComputeSHA1(String filePath) {
            FileStream fs = null;
            SHA1 sha1 = null;
            try {
                sha1 = SHA1Managed.Create();
                sha1.Initialize();
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
                sha1.ComputeHash(fs);
                return ChecksumUtils.ToHex(sha1.Hash).ToLower();
            }
            catch {
                throw;
            }
            finally {
                if (fs != null) {
                    fs.Dispose();
                }
                if (sha1 != null) {
                    sha1.Clear();
                }
            }
        }

        public static String ComputeSHA256(String filePath) {
            FileStream fs = null;
            SHA256 sha256 = null;
            try {
                sha256 = SHA256Managed.Create();
                sha256.Initialize();
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
                sha256.ComputeHash(fs);
                return ChecksumUtils.ToHex(sha256.Hash).ToLower();
            }
            catch {
                throw;
            }
            finally {
                if (fs != null) {
                    fs.Dispose();
                }
                if (sha256 != null) {
                    sha256.Clear();
                }
            }
        }
    }
}
