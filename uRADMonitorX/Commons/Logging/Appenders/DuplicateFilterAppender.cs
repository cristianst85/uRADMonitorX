// DuplicateFilterAppender.cs
// 
// The MIT License (MIT)
//
// Copyright (C) 2016, Cristian Stoica.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Timers;

namespace uRADMonitorX.Commons.Logging.Appenders
{
    public class DuplicateFilterAppender : ILoggerAppender, IDisposable
    {
        private readonly object _locker = new object();

        public bool Verbose { get; set; }

        private bool delay;
        public bool Delay
        {
            get
            {
                return delay;
            }
            set
            {
                lock (_locker)
                {
                    // If message delaying is disabled on the fly then append the last message.
                    if (this.delay && !value && this.lastMessage != null)
                    {
                        this.timer.Enabled = false;
                        this.appender.Append(lastMessage);

                    }
                    else if (this.MaxDelayInterval.TotalMilliseconds > 0)
                    {
                        this.timer.Enabled = true;
                    }

                    this.delay = value;
                }
            }
        }

        private TimeSpan maxDelayInterval = TimeSpan.FromSeconds(0);
        public TimeSpan MaxDelayInterval
        {
            get
            {
                return maxDelayInterval;
            }
            set
            {
                lock (_locker)
                {
                    this.maxDelayInterval = value;
                    this.timer.Interval = this.maxDelayInterval.TotalMilliseconds;

                    if (this.maxDelayInterval.TotalMilliseconds > 0)
                    {
                        if (!this.timer.Enabled)
                        {
                            this.timer.Enabled = true;
                        }
                    }
                    else
                    {
                        this.timer.Enabled = false;
                    }
                }
            }
        }

        private readonly ILoggerAppender appender;
        private Timer timer;

        private int lastMessageCount;
        private string lastMessage;

        public DuplicateFilterAppender(ILoggerAppender appender)
        {
            this.appender = appender;
            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(AppendLastMessage);
            this.timer.AutoReset = true;
        }

        public void Append(string message)
        {
            lock (this._locker)
            {
                if (this.lastMessage == null || !this.lastMessage.Equals(message))
                {
                    this.InternalAppend();

                    if (!this.Delay)
                    {
                        this.appender.Append(message);
                    }

                    this.lastMessage = message;
                    this.lastMessageCount = 1;
                }
                else
                {
                    lastMessageCount++;
                }
            }
        }

        private void AppendLastMessage(object sender, EventArgs e)
        {
            lock (_locker)
            {
                if (this.lastMessageCount > 0)
                {
                    this.InternalAppend();
                    this.lastMessageCount = 0;
                }
            }
        }

        private void InternalAppend()
        {
            if (this.lastMessage != null && this.Delay && !this.Verbose)
            {
                this.appender.Append(lastMessage);
            }
            else if (this.Delay && this.lastMessageCount == 1)
            {
                this.appender.Append(lastMessage);
            }

            if (this.Verbose && this.lastMessageCount > 1)
            {
                if (this.Delay)
                {
                    this.appender.Append(string.Format("{0} (repeated {1} times)", this.lastMessage, this.lastMessageCount));
                }
                else
                {
                    this.appender.Append(string.Format("Last message repeated {0} times", this.lastMessageCount));
                }
            }
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.timer != null)
                    {
                        this.timer.Stop();
                        this.timer.Elapsed -= AppendLastMessage;
                        this.timer.Dispose();

                        this.timer = null;
                    }

                    this.disposed = true;
                }
            }
        }
    }
}
