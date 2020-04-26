using System;
using System.IO;
using System.Net;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Updater
{
    public abstract class WebUpdater : IWebUpdater
    {
        public virtual IUpdateInfo Check(string updateUrl)
        {
            if (updateUrl.IsNullOrEmpty())
            {
                throw new ArgumentException("Update URL cannot be an null or an empty string.");
            }

            if (!updateUrl.StartsWith("HTTPS://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Non-secure update URL is not allowed.");
            }

            string fileContent = this.RetrieveContentFromUrl(updateUrl);

            return InternalCheck(fileContent);
        }

        protected abstract IUpdateInfo InternalCheck(string fileContent);

        public byte[] Download(string downloadUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(downloadUrl);
            request.UserAgent = Program.UserAgent;

            using (var ms = new MemoryStream())
            {
                using (var response = request.GetResponse())
                {
                    Stream data = response.GetResponseStream();

                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;

                    do
                    {
                        bytesRead = data.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, bytesRead);

                    } while (bytesRead > 0);
                }

                return ms.ToArray();
            }
        }

        protected virtual string RetrieveContentFromUrl(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = Program.UserAgent;

            string content = string.Empty;

            using (var response = request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    content = streamReader.ReadToEnd();
                }
            }

            return content;
        }
    }
}
