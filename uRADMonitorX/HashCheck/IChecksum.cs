namespace HashCheck
{
    public interface IChecksum
    {
        string Value { get; }

        bool Verify(string filePath);
    }
}
