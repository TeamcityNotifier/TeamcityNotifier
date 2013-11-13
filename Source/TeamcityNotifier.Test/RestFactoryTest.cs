﻿namespace TeamcityNotifier.Test
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
        private IXmlSerializer mockXmlSerializerProject;

        private project fakeProject;
        private projects1 fakeProject1;
        private IRestConfiguration configuration1;
        private IRestConfiguration configuration2;

        [SetUp]
        public void SetUp()
        {
            this.restConfigurations = new List<IRestConfiguration>();
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();

            fakeProject = new project();
            mockXmlSerializerProject = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(project))).Returns(mockXmlSerializerProject);

            fakeProject1 = new projects1();
            var mockXmlSerializerProject1 = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject1.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject1);
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
        public void CreateServer_WhenNoConfigurationExists_NoServerIsReturned()
        {
            this.restConfigurations.Clear();

            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(0);
        }

        [Test]
        public void CreateServer_WhenAServerIsConfigured_ThenReturnTheConfiguredServer()
        {
            this.restConfigurations.Clear();
            this.restConfigurations.Add(this.configuration1);
            var mockUri = A.Fake<IUri>();

            A.CallTo(() => this.mockWrapperFactory.CreateUri(A<string>.Ignored)).Returns(mockUri);

            var server = this.testee.CreateServers().FirstOrDefault();

            server.Should().NotBeNull("No server returned");
            server.Uri.Should().Be(mockUri, "Wrong uri is configured");
            server.UserName.Should().Be("user1", "Wrong username is configured");
            server.Password.Should().Be("password1", "Wrong password is configured");
        }

        [Test]
        public void CreateServer_WhenTwoServersAreConfigured_ThenReturnTwoServers()
        {
            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(2, "Not the correct number of servers is returned.");
        }

        [Test]
        public void CreateProjects_WhenNoProjectIsAvaiable_NoProjectIsReturned()
        {
            var mockServer = A.Fake<IServer>();

            var projects = this.testee.CreateProjects(mockServer);

            var projectCount = projects.Count();
            projectCount.Should().Be(0, string.Format("No server is available but {0} server(s) are returend.", projectCount));
        }

        [Test]
        public void CreateProjects_WhenOneProjectIsAvaiable_ThisProjectIsReturned()
        {
            var projectName = "testProject";
            var projectDescription = "projectDescription";
            this.fakeProject.name = projectName;
            this.fakeProject.description = projectDescription;

            this.fakeProject1.project.Add(new projectref());
            var mockServer = A.Fake<IServer>();

            var projects = this.testee.CreateProjects(mockServer);

            var projectCount = projects.Count();
            projects.FirstOrDefault().Name.Should().Be(projectName, "The project name is wrong.");
            projects.FirstOrDefault().Description.Should().Be(projectDescription, "The project description is wrong.");
        }   
        
        [Test]
        public void CreateProjects_WhenTwoProjectsAreAvaiable_TwoProjectsAreReturned()
        {
            this.fakeProject1.project.Add(new projectref());
            this.fakeProject1.project.Add(new projectref());
            var mockServer = A.Fake<IServer>();

            var projects = this.testee.CreateProjects(mockServer);

            var projectCount = projects.Count();
            projectCount.Should().Be(2, string.Format("Two projects are available but {0} are retuned.", projectCount));

        }

    }
}