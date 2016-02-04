using DKRUpdater.Core.Logging;
using DKRUpdater.Feeds.DKRModels;
using DKRUpdater.Feeds.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DKRUpdater.Feeds.Services
{
    // todo: create service that will read in the file, validate the properties, throwing errors and showing messages if needed
    // validate that the feed ids are unique, that all types are correct; that MaxFeedsToDownload is non-negative; that target playlists are .pls files; 
    // trim all strings and the filters on the commas

    public class FeedReaderService : IFeedReaderService
    {
        public List<IRetrievablePodcast> GetFeeds(string path)
        {
            if (!File.Exists(path))
            {
                var message = string.Format("Feed file at: '{0}' does not exist!", path);

                Log.Error(message, new Exception());

                throw new Exception(message);
            }

            var json = GetJsonFromPath(path);
            var downloadedableFeeds = GetDownloadableFeeds(path, json);

            var feedValidationService = new FeedValidationService();

            if (!feedValidationService.AreValidFeeds(downloadedableFeeds))
            {
                var message = "Feeds are not valid.";

                Log.Debug(message);

                throw new Exception();
            }

            return downloadedableFeeds;
        }

        private List<IRetrievablePodcast> GetDownloadableFeeds(string path, string json)
        {
            RssFeed downloadedableFeeds;

            try
            {
                Log.Debug("Starting deserialization of content from file at: '{0}'...", path);

                downloadedableFeeds = JsonConvert.DeserializeObject<RssFeed>(json);

                Log.Debug("Completed deserialization of content from file at: '{0}'.", path);


            }
            catch (Exception ex)
            {
                var message = string.Format("Failed to deserialize content at path: '{0}'", path);

                Log.Error(message, ex);

                throw new Exception(message, ex.InnerException);
            }

            return ConvertToRetrievablePodcastList(downloadedableFeeds.Feeds);
        }

        private static string GetJsonFromPath(string path)
        {
            var json = string.Empty;

            try
            {
                Log.Debug("Reading file: '{0}'", path);

                json = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to read file at: '{0}'", path), ex);
            }

            return json;
        }

        private List<IRetrievablePodcast> ConvertToRetrievablePodcastList(List<Feed> feeds)
        {
            var feedsToDownload = new List<IRetrievablePodcast>();

            foreach(var feed in feeds)
            {
                feedsToDownload.Add(feed);
            }

            return feedsToDownload;
        }
    }
}
