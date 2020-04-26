using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HashCheck
{
    public class MD5Checksum : IChecksum, IEquatable<MD5Checksum>
    {
        public static readonly int Length = 32;
        public static readonly Regex Regex = new Regex("^[a-f0-9]{32}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        public static MD5Checksum FromFile(string filePath)
        {
            return Compute(filePath);
        }

        public static MD5Checksum Parse(string value)
        {
            if (!Regex.IsMatch(value))
            {
                throw new Exception("Invalid MD5 hash format.");
            }

            return new MD5Checksum(value.ToLower());
        }

        private static MD5Checksum Compute(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                md5.Initialize();

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
                {
                    md5.ComputeHash(fileStream);

                    return new MD5Checksum(ToHex(md5.Hash).ToLower());
                }
            }
        }

        private static string ToHex(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        private MD5Checksum(string value)
        {
            this.Value = value;
        }

        public bool Verify(string filePath)
        {
            var fileChecksum = MD5Checksum.FromFile(filePath);

            return fileChecksum.Equals(this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as MD5Checksum);
        }

        public bool Equals(MD5Checksum other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.Value == other.Value;
            }
        }
    }
}
