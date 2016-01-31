using System;

namespace DKRUpdater.Core.StringParsing
{
    public class UrlParsing
    {
        public static string GetBaseUrl(Uri uri)
        {
            string baseUrl = uri.Scheme + "://" + uri.Authority;

            return baseUrl;
        }

        public static string GetPathFromUrl(Uri uri)
        {
            var basePath = GetBaseUrl(uri);

            var path = uri.ToString().Replace(basePath, string.Empty);

            return path.Remove(0, 1);
        }

        public static string GetFileNameFromUrl(Uri uri)
        {
            var partsOfUrl = uri.ToString().Split('/');

            var lastPartOfUrl = partsOfUrl[partsOfUrl.Length - 1];

            return lastPartOfUrl;
        }
    }
}
