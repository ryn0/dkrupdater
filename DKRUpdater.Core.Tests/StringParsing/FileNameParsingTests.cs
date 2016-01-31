using DKRUpdater.Core.StringParsing;
using System;
using Xunit;

namespace DKRUpdater.Core.Tests
{
    public class FileNameParsingTests
    {
        [Fact]
        public void CanGetDateFromPath()
        {
            // arrange
            var path = @"C:\MP3s\music\dancemachine5000\2015-04-04_1_dm5k27.mp3";
            var expectedDate = new DateTime(2015, 4, 4);

            // act
            var date = FileNameParsing.GetDateFromFilePath(path);

            // assert
            Assert.Equal(expectedDate, date);
        }
    }
}
