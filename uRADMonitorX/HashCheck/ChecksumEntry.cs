namespace HashCheck
{
    public class ChecksumEntry
    {
        public string FileName { get; private set; }

        public IChecksum Checksum { get; private set; }

        public ChecksumEntry(string fileName, IChecksum checksum)
        {
            this.FileName = fileName;
            this.Checksum = checksum;
        }
    }
}
