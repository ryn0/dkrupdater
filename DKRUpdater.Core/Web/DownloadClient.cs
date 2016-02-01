using DKRUpdater.Core.Logging;
using DKRUpdater.Core.StringParsing;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace DKRUpdater.Core.Web
{
    public class DownloadClient
    {
        public static T DownloadUrlContentIntoModel<T>(Uri uri)
        {
            Logger.Log(string.Format("Starting download string file at: '{0}'", uri));

            var content = GetcontentString(uri);
            var deserializedModel = ConvertToModel<T>(content);

            return deserializedModel;
        }

        public static bool DownloadFile(Uri uri, string destinationFilePath)
        {
            Logger.Log(string.Format("Starting download of file at: '{0}' to: '{1}'", uri, destinationFilePath));

            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(uri, destinationFilePath);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to download file '{0}'", uri), ex);

                return false;
            }

            Logger.Log(string.Format("Completed download of file at: '{0}' to: '{1}'", uri, destinationFilePath));

            return true;
        }

        private static string GetcontentString(Uri uri)
        {
            var content = string.Empty;

            try
            {
                var client = new RestClient(UrlParsing.GetBaseUrl(uri));

                var request = new RestRequest(UrlParsing.GetPathFromUrl(uri), Method.GET);

                var response = client.Execute(request);

                content = response.Content;
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to download string at: '{0}'", uri), ex);
            }

            Logger.Log(string.Format("Completed download string file at: '{0}'", uri));

            return content;
        }

        private static T ConvertToModel<T>(string content)
        {
            T deserializedModel = default(T);

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                deserializedModel = (T)xmlSerializer.Deserialize(new StringReader(content));
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to deserialize string: '{0}'", content), ex);
            }

            return deserializedModel;
        }

    }
}