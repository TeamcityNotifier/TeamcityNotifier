namespace TeamcityNotifier.Test
{
    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier.Wrapper;

    public class ServerTest
    {
        private IServer testee;

        private IWrapperFactory mockWrapperFactory;

        private IUri mockUri;

        [SetUp]
        public void SetUp()
        {
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();
            this.mockUri = A.Fake<IUri>();
            A.CallTo(() => this.mockWrapperFactory.CreateUri(A<string>.Ignored)).Returns(this.mockUri);

            var mockConfiguration = A.Fake<IRestConfiguration>();
            A.CallTo(() => mockConfiguration.UserName).Returns("userName");
            A.CallTo(() => mockConfiguration.Password).Returns("password");
            A.CallTo(() => mockConfiguration.Name).Returns("servername");

            this.testee = new Server(this.mockWrapperFactory, mockConfiguration);
        }

        [Test]
        public void CreateNewServer_WhenParametersAreSet_ThePropertiesAreSetCorrectliy()
        {
            this.testee.Uri.Should().Be(this.mockUri);
            this.testee.UserName.Should().Be("userName", "wrong user name is set");
            this.testee.Password.Should().Be("password", "wrong password is set");
            this.testee.Name.Should().Be("servername", "wrong server name is set");
        } 
    }
}