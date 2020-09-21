using System;
using FluentAssertions;
using Xunit;

namespace SeriesAndEpisodes.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassingTest()
        {
            var number = 1;
            number.Should().Be(1);
        }
    }
}
