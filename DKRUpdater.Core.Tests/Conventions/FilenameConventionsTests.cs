using DKRUpdater.Core.Conventions;
using DKRUpdater.Core.Enums;
using System;
using Xunit;

namespace DKRUpdater.Core.Tests
{
    public class FilenameConventionsTests
    {
        [Fact]
        public void CanCreateCorrectFileName()
        {
            // arrange
            var uri = new Uri("http://dancemachine5000.files.wordpress.com/2016/01/dm5k36.m4a");
            var origin = PodcastFeedOrigin.DanceMachine5000;
            var pubDate = Convert.ToDateTime("Thu, 21 Jan 2016 01:49:12 +0000");

            // act
            var formattedFilename = FilenameConventions.DownloadFilenameFormatter(uri, origin, pubDate);

            // assert
            Assert.Equal("2016-01-20_1_dm5k36.m4a", formattedFilename);
        }
    }
}