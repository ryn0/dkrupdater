using DKRUpdater.Core.StringParsing;
using System;
using Xunit;

namespace DKRUpdater.Core.Tests
{
    public class UrlParsingTests
    {
        [Fact]
        public void CanGetBaseUrl()
        {
            // arrange
            var uri = new Uri("http://dancemachine5000.com/feed/");

            // act
            var baseUrl = UrlParsing.GetBaseUrl(uri);

            // assert
            Assert.Equal("http://dancemachine5000.com", baseUrl);
        }

        [Fact]
        public void CanGetPathFromUrl()
        {
            // arrange
            var uri = new Uri("http://dancemachine5000.com/feed/");

            // act
            var path = UrlParsing.GetPathFromUrl(uri);

            // assert
            Assert.Equal("feed/", path);
        }

        [Fact]
        public void CanGetFileFromUrl()
        {
            // arrange
            var uri = new Uri("http://dancemachine5000.files.wordpress.com/2016/01/dm5k36.m4a");

            // act
            var path = UrlParsing.GetFileNameFromUrl(uri);

            // assert
            Assert.Equal("dm5k36.m4a", path);
        }
    }
}
