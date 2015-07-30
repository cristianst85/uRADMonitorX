using System;
using System.Collections.Generic;

namespace uRADMonitorX.Commons.Networking {

    internal class HttpStatus {

        private readonly static IList<HttpStatus> statuses = new List<HttpStatus>() {
            HttpStatus.OK
        };

        public int Code { get; private set; }
        public String Reason { get; private set; }

        private HttpStatus(int code, String reason) {
            this.Code = code;
            this.Reason = reason;
        }

        public static HttpStatus OK {
            get {
                return new HttpStatus(200, "OK");
            }
        }

        public static HttpStatus FromCode(int code) {
            foreach (HttpStatus status in statuses) {
                if (status.Code == code) {
                    return status;
                }
            }
            throw new Exception(String.Format("Http status code {0} was not found.", code));
        }

        public static String GetReason(int code) {
            foreach (HttpStatus status in statuses) {
                if (status.Code == code) {
                    return status.Reason;
                }
            }
            return null;
        }
    }
}
