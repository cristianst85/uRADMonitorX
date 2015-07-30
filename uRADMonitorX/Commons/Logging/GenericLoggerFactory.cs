namespace uRADMonitorX.Commons.Logging {

    public class GenericLoggerFactory<T> where T : ILogger, new() {

        public GenericLoggerFactory() {
        }

        public T Create() {
            return new T();
        }
    }
}
