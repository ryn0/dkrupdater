using System;
using System.Collections.Generic;
using DKRUpdater.Feeds.Interfaces;
using DKRUpdater.Core.Logging;

namespace DKRUpdater.Feeds.Services
{
    public class FeedValidationService : IFeedValidationService
    {
        public bool AreValidFeeds(List<IRetrievablePodcast> podcast)
        {
            HasUniqueIds(podcast);

            return true;
        }

        private static void HasUniqueIds(List<IRetrievablePodcast> podcast)
        {
            var uniqueFeedIds = new List<int>();

            foreach (var feed in podcast)
            {
                if (uniqueFeedIds.Contains(feed.FeedId))
                {
                    var message = string.Format("Feed id: '{0}' was already added", feed.FeedId);

                    Log.Error(message, new Exception());

                    throw new Exception(message);
                }

                uniqueFeedIds.Add(feed.FeedId);
            }
        }
    }
}