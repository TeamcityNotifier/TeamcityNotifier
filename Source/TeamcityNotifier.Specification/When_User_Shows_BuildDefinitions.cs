namespace TeamcityNotifier.Specification
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using Machine.Specifications;

    using TeamcityNotifier.Wrapper;

    using DataAbstraction;

    public class When_User_Shows_BuildDefinitions
    {
        private static IService service;


        private Establish context = () =>
        {
            var mockWrapperFactory = A.Fake<IWrapperFactory>();

            var fakeBuildDefinition = new buildType(){name = "buildDefinitionName", description = "buildDefinitionDescription"};
            var mockXmlSerializerBuildType = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerBuildType.Deserialize(A<IStringReader>.Ignored)).Returns(fakeBuildDefinition);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(buildType)))
                .Returns(mockXmlSerializerBuildType);

            var fakeProject = new project();
            var mockXmlSerializerProject = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(project)))
                .Returns(mockXmlSerializerProject);

            var fakeProject1 = new projects1();
            var mockXmlSerializerProject1 = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject1.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject1);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(projects1)))
                .Returns(mockXmlSerializerProject1);

            var build = new builds();
            var mockXmlSerializerbuild = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerbuild.Deserialize(A<IStringReader>.Ignored)).Returns(build);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(builds)))
                .Returns(mockXmlSerializerbuild);

            fakeProject1.project.Add(new projectref());
            fakeProject1.project.Add(new projectref());

            fakeProject.buildTypes.Add(new buildTyperef());
            fakeProject.buildTypes.Add(new buildTyperef());

            var restConfigurations = new List<IRestConfiguration>();

            var configuration = A.Fake<IRestConfiguration>();
            A.CallTo(() => configuration.BaseUrl).Returns("url1");
            A.CallTo(() => configuration.UserName).Returns("user1");
            A.CallTo(() => configuration.Password).Returns("password1");
            restConfigurations.Add(configuration);


            configuration = A.Fake<IRestConfiguration>();
            A.CallTo(() => configuration.BaseUrl).Returns("url2");
            A.CallTo(() => configuration.UserName).Returns("user2");
            A.CallTo(() => configuration.Password).Returns("password2");
            restConfigurations.Add(configuration);

            var networkFactory = new NetworkFactory();

            var factory = new RestFactory(restConfigurations, mockWrapperFactory, networkFactory);

            service = new Service(factory, networkFactory);
        };

        private It should_returne_the_available_builddefinitions = () =>
        {
            var servers = service.GetServers();
            servers.Count().Should().BeGreaterOrEqualTo(1, "No server retrned!");
            
            var projects = servers.FirstOrDefault().ProjectRepository.Projects;
            projects.Count().Should().Be(2, "Wrong count of projects ist returned");


            var buildDefinitions = projects.FirstOrDefault().BuildDefinitions;
            buildDefinitions.Should().NotBeNull("no build definitions ist returned");
            buildDefinitions.Count().Should().Be(2, "Wrong count of build definitions ist returned");

            var buildDefinition = buildDefinitions.FirstOrDefault();
            buildDefinition.Name.Should().Be("buildDefinitionName", "the wrong name is set");
            buildDefinition.Description.Should().Be("buildDefinitionDescription", "the wrong description is set");
        }; 
    }
}