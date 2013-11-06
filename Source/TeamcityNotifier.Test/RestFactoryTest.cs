namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier;
    using TeamcityNotifier.Wrapper;

    using xsdtest;

    public class RestFactoryTest
    {
        private IFactory testee;

        private IList<IRestConfiguration> restConfigurations;

        private IWrapperFactory mockWrapperFactory;

        private project project;
        private projects1 project1;
        private IRestConfiguration configuration1;
        private IRestConfiguration configuration2;

        [SetUp]
        public void SetUp()
        {
            this.restConfigurations = new List<IRestConfiguration>();
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();

            project = new project();
            project1 = new projects1();

            var mockXmlSerializerProject = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject.Deserialize(A<IStringReader>.Ignored)).Returns(project);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(project))).Returns(mockXmlSerializerProject);

            var mockXmlSerializerProject1 = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject1.Deserialize(A<IStringReader>.Ignored)).Returns(project1);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(projects1))).Returns(mockXmlSerializerProject1);


            this.configuration1 = A.Fake<IRestConfiguration>();
            A.CallTo(() => this.configuration1.BaseUrl).Returns("url1");
            A.CallTo(() => this.configuration1.UserName).Returns("user1");
            A.CallTo(() => this.configuration1.Password).Returns("password1");
            restConfigurations.Add(this.configuration1);


            this.configuration2 = A.Fake<IRestConfiguration>();
            A.CallTo(() => this.configuration2.BaseUrl).Returns("url2");
            A.CallTo(() => this.configuration2.UserName).Returns("user2");
            A.CallTo(() => this.configuration2.Password).Returns("password2");
            restConfigurations.Add(this.configuration2);

            
            this.testee = new RestFactory(this.restConfigurations, this.mockWrapperFactory);
        }

        [Test]
        public void CreateServer_WhenTwoServersAreConfigured_ThenReturnTwoServers()
        {
            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(2, "Not the correct number of servers is returned.");
        }

        [Test]
        public void CreateServer_WhenAServerisConfigured_ThenReturnTheConfiguredServer()
        {
            restConfigurations.Clear();
            restConfigurations.Add(this.configuration1);

            var server = this.testee.CreateServers().FirstOrDefault();

            server.Should().NotBeNull("No server returned");
            server.UserName.Should().Be("user1", "Wrong username is configured");
            server.Password.Should().Be("password1", "Wrong password is configured");
        }
        
    }
}