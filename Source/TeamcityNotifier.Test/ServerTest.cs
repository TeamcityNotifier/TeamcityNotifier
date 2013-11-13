namespace TeamcityNotifier.Test
{
    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier.Wrapper;

    public class ServerTest
    {
        private IServer testee;

        private IUri mockUri;

        private string userName = "userName";

        private string password = "password";

        [SetUp]
        public void SetUp()
        {
            this.mockUri = A.Fake<IUri>();
            this.testee = new Server(this.mockUri, userName, password);
        }

        [Test]
        public void CreateNewServer_WhenParametersAreSet_ThePropertiesAreSetCorrectliy()
        {
            this.testee.Uri.Should().Be(this.mockUri);
            this.testee.UserName.Should().Be(userName);
            this.testee.Password.Should().Be(password);
        } 
    }
}