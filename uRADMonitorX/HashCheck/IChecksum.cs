using System;

namespace HashCheck {

    public interface IChecksum {

        String Value { get; }

        bool Verify(String filePath);
    }
}
