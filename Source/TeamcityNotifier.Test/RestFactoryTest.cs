namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public class RestFactoryTest
    {
        private IRestFactory testee;

        private IList<IRestConfiguration> restConfigurations;
        private IWrapperFactory mockWrapperFactory;
        private IRestConfiguration configuration1;
        private IRestConfiguration configuration2;

        private INetworkFactory mockNetworkFactory;

        private IHttpClient mockHttpClientHandler;

        private IRestConsumer mockRestConsumer;

        [SetUp]
        public void SetUp()
        {
            this.restConfigurations = new List<IRestConfiguration>();
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();
            this.mockNetworkFactory = A.Fake<INetworkFactory>();

            this.configuration1 = A.Fake<IRestConfiguration>();
            A.CallTo(() => this.configuration1.BaseUrl).Returns("url1");
            A.CallTo(() => this.configuration1.UserName).Returns("user1");
            A.CallTo(() => this.configuration1.Password).Returns("password1");
            A.CallTo(() => this.configuration1.Name).Returns("conf1");

            this.configuration2 = A.Fake<IRestConfiguration>();
            A.CallTo(() => this.configuration2.BaseUrl).Returns("url2");
            A.CallTo(() => this.configuration2.UserName).Returns("user2");
            A.CallTo(() => this.configuration2.Password).Returns("password2");
            A.CallTo(() => this.configuration1.Name).Returns("conf2");

            this.mockHttpClientHandler = A.Fake<IHttpClient>();
            this.mockRestConsumer = A.Fake<IRestConsumer>();

            A.CallTo(() => this.mockWrapperFactory.CreateHttpClientHandler(this.configuration1.UserName, this.configuration1.Password)).Returns(mockHttpClientHandler);
            A.CallTo(() => this.mockNetworkFactory.CreateRestConsumer(A<IUri>.Ignored, mockHttpClientHandler, mockWrapperFactory)).Returns(mockRestConsumer);

            this.testee = new RestFactory(this.restConfigurations, this.mockWrapperFactory, mockNetworkFactory);
        }

        [Test]
        public void CreateServer_WhenNoConfigurationExists_NoServerIsReturned()
        {
            this.restConfigurations.Clear();

            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(0);
        }

        [Test]
        public void CreateServer_WhenAServerIsConfigured_ThenReturnTheConfiguredServer()
        {
            this.restConfigurations.Add(this.configuration1);

            var mockUri = A.Fake<IUri>();
            A.CallTo(() => this.mockWrapperFactory.CreateUri(this.configuration1.BaseUrl)).Returns(mockUri);

            var server = this.testee.CreateServers().FirstOrDefault();

            server.Should().NotBeNull("No server returned");
            server.Uri.Should().Be(mockUri, "Wrong uri is configured");
        }

        [Test]
        public void CreateServer_WhenTwoServersAreConfigured_ThenReturnTwoServers()
        {
            this.restConfigurations.Add(this.configuration1);
            this.restConfigurations.Add(this.configuration2);

            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(2, "Not the correct number of servers is returned.");
        }

        [Test]
        public void CreateProjectRepository_WhenRepositoryIsLoaded_RestConsumerIsUsed()
        {
            var mockServer = A.Fake<IServer>();

            A.CallTo(() => mockServer.RestConsumer).Returns(this.mockRestConsumer);

            this.testee.GetProjectRepository(mockServer);

            A.CallTo(() => this.mockRestConsumer.Load(A<ProjectRepository>.Ignored)).MustHaveHappened();
        }
        
    }
}