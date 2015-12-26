using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HashCheck {

    public class ReadOnlyMD5File : IReadOnlyChecksumFile {

        protected List<ChecksumEntry> entries = new List<ChecksumEntry>();
        public ReadOnlyCollection<ChecksumEntry> Entries {
            get {
                return new ReadOnlyCollection<ChecksumEntry>(entries);
            }
        }

        private ReadOnlyMD5File(ICollection<ChecksumEntry> entries) {
            this.entries.AddRange(entries);
        }

        public static IReadOnlyChecksumFile Load(String checksumFileContent) {
            String[] lines = checksumFileContent.Split(new string[] { "\r", "\n" } , StringSplitOptions.RemoveEmptyEntries);
            IList<ChecksumEntry> entries = new List<ChecksumEntry>();
            IList<String> comments = new List<String>();

            foreach (String line in lines) {
                if (line.Trim().Length == 0 || line.StartsWith(";")) {
                    ; // Ignore empty lines, lines only with white spaces and comments.
                }
                else {
                    String checksum = line.Trim().Substring(0, MD5Checksum.Length);
                    String filename = line.Trim().Substring(checksum.Length).Trim().TrimStart('*');
                    entries.Add(new ChecksumEntry(filename, MD5Checksum.Parse(checksum)));
                }
            }
            return new ReadOnlyMD5File(entries);
        }
    }
}
