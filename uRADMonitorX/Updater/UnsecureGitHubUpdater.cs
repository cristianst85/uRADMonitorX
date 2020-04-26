using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Updater
{
    public class UnsecureGitHubUpdater : GitHubUpdater
    {
        public UnsecureGitHubUpdater()
        {
        }

        public override IUpdateInfo Check(string updateUrl)
        {
            ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertificates;

            if (updateUrl.IsNullOrEmpty())
            {
                throw new ArgumentException("Update URL cannot be an null or an empty string.");
            }

            string fileContent = base.RetrieveContentFromUrl(updateUrl);

            return InternalCheck(fileContent);
        }

        private static bool AcceptAllCertificates(object sender, X509Certificate certification, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
