using System;
using System.Security.Cryptography;
using System.Text;

namespace uRADMonitorX.Commons.Cryptography
{
    public static class DataProtectionApiWrapper
    {
        /// <summary>
        /// Specifies the data protection scope of the DPAPI.
        /// </summary>
        private const DataProtectionScope Scope = DataProtectionScope.CurrentUser;

        public static String Encrypt(String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // Encrypt data.
            var data = Encoding.Unicode.GetBytes(text);
            byte[] encrypted = ProtectedData.Protect(data, null, Scope);

            // Return as Base 64 string.
            return Convert.ToBase64String(encrypted);
        }

        public static String Decrypt(String cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }

            // Parse Base 64 string.
            byte[] data = Convert.FromBase64String(cipher);

            // Decrypt data.
            byte[] decrypted = ProtectedData.Unprotect(data, null, Scope);

            return Encoding.Unicode.GetString(decrypted);
        }
    }
}
