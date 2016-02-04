using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKRUpdater.Feeds.Interfaces
{
    public interface IFeedReaderService
    {
        List<IRetrievablePodcast> GetFeeds(string path);
    }
}
