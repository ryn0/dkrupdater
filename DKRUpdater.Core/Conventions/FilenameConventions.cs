using DKRUpdater.Core.Constants;
using DKRUpdater.Core.StringParsing;
using System;

namespace DKRUpdater.Core.Conventions
{
    public class FilenameConventions
    {
        public static string DownloadFilenameFormatter(
                Uri uriOfFile, 
                int feedId,
                DateTime podcastPublishDate)
        {            
            var format = "{0}_{1}_{2}";
            var dateFormatted = podcastPublishDate.Year.ToString("0000") +
                                "-" +
                                podcastPublishDate.Month.ToString("00") +
                                "-" +
                                podcastPublishDate.Day.ToString("00");

            var fileNameFromUrl = UrlParsing.GetFileNameFromUrl(uriOfFile);

            var cleanedFileName = FileNameParsing.CleanFileName(fileNameFromUrl);

            if (cleanedFileName.EndsWith(StringConstants.m4a))
            {
                // this represents the final file format that should exist
                cleanedFileName = cleanedFileName.Replace(StringConstants.m4a, StringConstants.mp3);
            }

            var fileName = string.Format(format, dateFormatted, feedId, cleanedFileName);

            return fileName;
        }
    }
}
