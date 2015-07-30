﻿using System;
using System.Collections.Generic;

namespace uRADMonitorX.Commons.Logging {

    internal class LoggerManager {

        private static object _locker = new object();
        private static object _syncRoot = new object();
        private static volatile LoggerManager _instance;

        public static LoggerManager GetInstance() {
            if (_instance == null) {
                lock (_syncRoot) {
                    if (_instance == null)
                        _instance = new LoggerManager();
                }
            }
            return _instance;
        }

        protected object locker = new object();
        protected IDictionary<String, ILogger> loggers = new Dictionary<String, ILogger>();

        protected LoggerManager() {
        }

        protected LoggerManager(ILogger defaultLogger) {
            this.loggers.Add(new KeyValuePair<string, ILogger>("default", defaultLogger));
        }

        /// <summary>
        /// Registers the default logger.
        /// Throws an exception if default logger was already registered.
        /// </summary>
        /// <param name="logger"></param>
        public virtual void Add(ILogger logger) {
            this.Add("default", logger);
        }

        /// <summary>
        /// Registers a logger with the specified name.
        /// Throws an exception if a logger instance with the same name already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logger"></param>
        public virtual void Add(String name, ILogger logger) {
            lock (locker) {
                if (!this.loggers.ContainsKey(name)) {
                    this.loggers.Add(new KeyValuePair<string, ILogger>(name, logger));
                }
                else {
                    throw new Exception(String.Format("A logger instance with the name '{0}' already exists.", name));
                }
            }
        }

        /// <summary>
        /// Returns a reference to the default logger instance.
        /// Throws an exception if a default logger does not exists.
        /// </summary>
        /// <returns></returns>
        public virtual ILogger GetLogger() {
            return this.GetLogger("default");
        }

        /// <summary>
        /// Returns a reference to a logger instance with the specified name.
        /// Throws an exception if a logger instance with the specified name does not exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ILogger GetLogger(String name) {
            lock (locker) {
                if (this.loggers.ContainsKey(name)) {
                    return this.loggers[name];
                }
                else {
                    throw new Exception(String.Format("Logger with the name '{0}' was not found.", name));
                }
            }
        }
    }
}
