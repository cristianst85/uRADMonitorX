using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public class MemoryAppender : ILoggerAppender {

        public ICollection<String> Messages { get; private set; }

        public MemoryAppender() {
            this.Messages = new Collection<String>();
        }

        public void Append(string message) {
            this.Messages.Add(message);
        }
    }
}
