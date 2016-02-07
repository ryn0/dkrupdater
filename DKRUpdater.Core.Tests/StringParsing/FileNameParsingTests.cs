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

        [Fact]
        public void CleanFileName()
        {
            // arrange
            var fileName = @"2016-01-17_8_2016: GOTH, INDUSTRIAL, EBM, SYNTHPOP, ELECTRONIC, TECHNO.mp3";

            // act
            var clearnFileName = FileNameParsing.CleanFileName(fileName);

            // assert
            Assert.Equal(@"2016-01-17_8_2016 GOTH, INDUSTRIAL, EBM, SYNTHPOP, ELECTRONIC, TECHNO.mp3", clearnFileName);
        }
    }
}
