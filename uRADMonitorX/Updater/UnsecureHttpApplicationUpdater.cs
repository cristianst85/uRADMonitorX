using System;
using System.Net;

namespace uRADMonitorX.Updater {

    public class UnsecureHttpApplicationUpdater : HttpApplicationUpdater {

        public UnsecureHttpApplicationUpdater(String updateUrl)
            : base(updateUrl) {
        }

        public override IApplicationUpdateInfo Check() {
            ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertificates;
            return base.Check();
        }

        private static bool AcceptAllCertificates(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) {
            return true;
        }
    }
}
