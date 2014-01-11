namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;

    using FakeItEasy;

    using NUnit.Framework;

    using TeamcityNotifier.RestObject;

    public class ServiceTest
    {
        private IService testee;

        private IRestFactory mockRestFactory;

        private INetworkFactory mockNetworkFactory;

        [SetUp]
        public void SetUp()
        {
            this.mockRestFactory = A.Fake<IRestFactory>();

            this.mockNetworkFactory = A.Fake<INetworkFactory>();

            this.testee = new Service(this.mockRestFactory, this.mockNetworkFactory);
        }

        [Test]
        public void GetServers_WhenGettingServers_UsesRestFactoryToGetServerInstances()
        {
            this.testee.GetServers();

            A.CallTo(() => this.mockRestFactory.CreateServers()).MustHaveHappened();
        }

        [Test]
        public void StartPeriodicallyUpdating_WhenStartingUpdates_UsesNetworkFactoryToGetUpdaterInstance()
        {
            var mockRestConsumer = A.Fake<IRestConsumer>();

            var mockServer = A.Fake<IServer>();
            A.CallTo(() => mockServer.RestConsumer).Returns(mockRestConsumer);

            this.testee.StartPeriodicallyUpdating(mockServer);

            A.CallTo(() => this.mockNetworkFactory.CreateUpdater(mockRestConsumer)).MustHaveHappened();
        }

        [Test]
        public void StartPeriodicallyUpdating_WhenStartingUpdatesWithOneServer_RegisterAllBuildRepositoryForUpdates()
        {
            var mockRestConsumer = A.Fake<IRestConsumer>();

            var mockBuildRepository1 = A.Fake<IBuildRepository>();
            var mockBuildRepository2 = A.Fake<IBuildRepository>();

            var mockBuildDefinition1 = A.Fake<IBuildDefinition>();
            A.CallTo(() => mockBuildDefinition1.BuildRepository).Returns(mockBuildRepository1);

            var mockBuildDefinition2 = A.Fake<IBuildDefinition>();
            A.CallTo(() => mockBuildDefinition2.BuildRepository).Returns(mockBuildRepository2);

            var mockBuildDefinitions = new List<IBuildDefinition> { mockBuildDefinition1, mockBuildDefinition2 };

            var mockProject = A.Fake<IProject>();
            A.CallTo(() => mockProject.BuildDefinitions).Returns(mockBuildDefinitions);

            var mockProjects = new List<IProject> { mockProject };

            var mockProjectRepository = A.Fake<IProjectRepository>();
            A.CallTo(() => mockProjectRepository.Projects).Returns(mockProjects);

            var mockServer = A.Fake<IServer>();
            A.CallTo(() => mockServer.RestConsumer).Returns(mockRestConsumer);
            A.CallTo(() => mockServer.ProjectRepository).Returns(mockProjectRepository);

            var mockUpdater = A.Fake<IUpdater>();
            A.CallTo(() => this.mockNetworkFactory.CreateUpdater(mockRestConsumer)).Returns(mockUpdater);

            this.testee.StartPeriodicallyUpdating(mockServer);

            A.CallTo(() => mockUpdater.Register(mockBuildRepository1)).MustHaveHappened();
            A.CallTo(() => mockUpdater.Register(mockBuildRepository2)).MustHaveHappened();
        }

        [Test]
        public void StartPeriodicallyUpdating_WhenStartingUpdatesWithTwoServers_RegisterAllBuildRepositoryForUpdates()
        {
            var mockRestConsumer = A.Fake<IRestConsumer>();

            var mockBuildRepository1 = A.Fake<IBuildRepository>();
            var mockBuildRepository2 = A.Fake<IBuildRepository>();

            var mockBuildDefinition1 = A.Fake<IBuildDefinition>();
            A.CallTo(() => mockBuildDefinition1.BuildRepository).Returns(mockBuildRepository1);

            var mockBuildDefinition2 = A.Fake<IBuildDefinition>();
            A.CallTo(() => mockBuildDefinition2.BuildRepository).Returns(mockBuildRepository2);

            var mockBuildDefinitions = new List<IBuildDefinition> { mockBuildDefinition1, mockBuildDefinition2 };

            var mockProject = A.Fake<IProject>();
            A.CallTo(() => mockProject.BuildDefinitions).Returns(mockBuildDefinitions);

            var mockProjects = new List<IProject> { mockProject };

            var mockProjectRepository = A.Fake<IProjectRepository>();
            A.CallTo(() => mockProjectRepository.Projects).Returns(mockProjects);

            var mockServer1 = A.Fake<IServer>();
            A.CallTo(() => mockServer1.RestConsumer).Returns(mockRestConsumer);
            A.CallTo(() => mockServer1.ProjectRepository).Returns(mockProjectRepository);

            var mockUpdater = A.Fake<IUpdater>();
            A.CallTo(() => this.mockNetworkFactory.CreateUpdater(mockRestConsumer)).Returns(mockUpdater);

            this.testee.StartPeriodicallyUpdating(new List<IServer> { mockServer1, mockServer1 });

            A.CallTo(() => mockUpdater.Register(mockBuildRepository1)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => mockUpdater.Register(mockBuildRepository2)).MustHaveHappened(Repeated.Exactly.Twice);
        }

    }
}