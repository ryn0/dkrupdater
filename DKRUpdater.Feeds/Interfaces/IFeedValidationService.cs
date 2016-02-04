using System.Collections.Generic;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IFeedValidationService
    {
        bool AreValidFeeds(List<IRetrievablePodcast> podcast);
    }
}
