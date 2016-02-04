namespace DKRUpdater.Feeds.Interfaces
{
    public interface IFeedValidationService
    {
        bool IsValid(IRetrievablePodcast podcast);
    }
}
