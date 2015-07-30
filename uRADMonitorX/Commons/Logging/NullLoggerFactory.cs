namespace uRADMonitorX.Commons.Logging {

    public class NullLoggerFactory : ILoggerFactory {

        public NullLoggerFactory() {
        }

        public ILogger Create() {
            return new NullLogger();
        }
    }
}
