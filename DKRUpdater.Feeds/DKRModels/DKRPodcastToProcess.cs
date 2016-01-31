using System;
using System.Collections.Generic;

namespace DKRUpdater.Feeds.DKRModels
{
    public class DKRPodcastFileToProcess
    {
        public string DestinationPathForMp3 { get; set; }

        public string PathToDownloadedMp3 { get; set; }

        public List<string> PlaylistPathsToIncludeIn { get; set; }

        public DateTime ReleaseDateOfPodcastFile { get; set; }
    }
}