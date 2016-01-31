using DKRUpdater.Core.Enums;
using DKRUpdater.Core.StringParsing;
using System;

namespace DKRUpdater.Core.Conventions
{
    public class FilenameConventions
    {
        public static string DownloadFilenameFormatter(
                Uri uriOfFile, 
                PodcastFeedOrigin feedOrigin,
                DateTime podcastPublishDate)
        {            
            var format = "{0}_{1}_{2}";
            var dateFormatted = podcastPublishDate.Year.ToString("0000") +
                                "-" +
                                podcastPublishDate.Month.ToString("00") +
                                "-" +
                                podcastPublishDate.Day.ToString("00");

            var originFormatted = ((int)feedOrigin).ToString();

            var fileNameFromUrl = UrlParsing.GetFileNameFromUrl(uriOfFile);

            var fileName = string.Format(format, dateFormatted, originFormatted, fileNameFromUrl);

            return fileName;
        }
    }
}
