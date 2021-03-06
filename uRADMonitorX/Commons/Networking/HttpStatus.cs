﻿using System;
using System.Collections.Generic;

namespace uRADMonitorX.Commons.Networking
{
    public class HttpStatus
    {
        private readonly static IList<HttpStatus> statuses = new List<HttpStatus>() {
            HttpStatus.OK
        };

        public int Code { get; private set; }
        public string Reason { get; private set; }

        private HttpStatus(int code, string reason)
        {
            this.Code = code;
            this.Reason = reason;
        }

        public static HttpStatus Unreachable
        {
            get
            {
                return new HttpStatus(0, "Unreachable");
            }
        }

        public static HttpStatus OK
        {
            get
            {
                return new HttpStatus(200, "OK");
            }
        }

        public static HttpStatus InternalServerError
        {
            get
            {
                return new HttpStatus(500, "Internal Server Error");
            }
        }

        public static HttpStatus ServiceUnavailable
        {
            get
            {
                return new HttpStatus(503, "Service Unavailable");
            }
        }

        public static HttpStatus FromCode(int code)
        {
            foreach (HttpStatus status in statuses)
            {
                if (status.Code == code)
                {
                    return status;
                }
            }
            throw new Exception(string.Format("HTTP status code {0} was not found.", code));
        }

        public static string GetReason(int code)
        {
            foreach (var status in statuses)
            {
                if (status.Code == code)
                {
                    return status.Reason;
                }
            }

            return null;
        }
    }
}
