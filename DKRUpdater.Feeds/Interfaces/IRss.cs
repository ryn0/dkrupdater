using DKRUpdater.Feeds.Podcasts.BaseRss;

namespace DKRUpdater.Feeds.Podcasts.Interfaces
{
    public interface IRss
    {

        Channel Channel { get; set; }

        string Version { get; set; }

        string Content { get; set; }

        string Wfw { get; set; }

        string Dc { get; set; }

        string Atom { get; set; }

        string Itunes { get; set; }

        string Media { get; set; }

    }
}