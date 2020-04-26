using System.Collections.ObjectModel;

namespace HashCheck
{
    public interface IReadOnlyChecksumFile
    {
        ReadOnlyCollection<ChecksumEntry> Entries { get; }
    }
}
