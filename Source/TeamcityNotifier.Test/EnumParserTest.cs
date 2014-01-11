namespace TeamcityNotifier.Test
{
    using NUnit.Framework;
    using FluentAssertions;

    using TeamcityNotifier.RestObject;

    [TestFixture]
    public class EnumParserTest
    {
        [Test]
        public void GetStatusFor_Error_ReturnsError()
        {
            var statusToParse = "ERROR";

            var parsedStatus = EnumParser.GetStatusFor(statusToParse);

            parsedStatus.Should().Be(Status.Error);
        }

        [Test]
        public void GetStatusFor_Failure_ReturnsFailure()
        {
            var statusToParse = "FAILURE";

            var parsedStatus = EnumParser.GetStatusFor(statusToParse);

            parsedStatus.Should().Be(Status.Failure);
        }

        [Test]
        public void GetStatusFor_Success_ReturnsSuccess()
        {
            var statusToParse = "SUCCESS";

            var parsedStatus = EnumParser.GetStatusFor(statusToParse);

            parsedStatus.Should().Be(Status.Success);
        }
    }
}