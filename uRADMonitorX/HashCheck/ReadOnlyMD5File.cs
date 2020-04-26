using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HashCheck
{
    public class ReadOnlyMD5File : IReadOnlyChecksumFile
    {
        protected List<ChecksumEntry> entries = new List<ChecksumEntry>();

        public ReadOnlyCollection<ChecksumEntry> Entries
        {
            get
            {
                return new ReadOnlyCollection<ChecksumEntry>(entries);
            }
        }

        private ReadOnlyMD5File(ICollection<ChecksumEntry> entries)
        {
            this.entries.AddRange(entries);
        }

        public static IReadOnlyChecksumFile Load(string checksumFileContent)
        {
            string[] lines = checksumFileContent.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var checksumEntries = new List<ChecksumEntry>();

            foreach (var line in lines)
            {
                if (line.Trim().Length == 0 || line.StartsWith(";"))
                {
                    ; // Ignore empty lines, lines containing only white spaces or comments.
                }
                else
                {
                    string checksum = line.Trim().Substring(0, MD5Checksum.Length);
                    string fileName = line.Trim().Substring(checksum.Length).Trim().TrimStart('*');

                    checksumEntries.Add(new ChecksumEntry(fileName, MD5Checksum.Parse(checksum)));
                }
            }

            return new ReadOnlyMD5File(checksumEntries);
        }
    }
}
