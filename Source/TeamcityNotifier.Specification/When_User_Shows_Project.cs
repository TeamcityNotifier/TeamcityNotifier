namespace TeamcityNotifier.Specification
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using Machine.Specifications;

    using TeamcityNotifier.Wrapper;

    using xsdtest;

    public class When_User_Shows_Project
    {
        private static IService service;


        private Establish context = () =>
        {
            var mockWrapperFactory = A.Fake<IWrapperFactory>();

            var fakeProject = new project(){name = "projectName", description = "projectDescription"};
            var mockXmlSerializerProject = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(project)))
                .Returns(mockXmlSerializerProject);

            var fakeProject1 = new projects1();
            var mockXmlSerializerProject1 = A.Fake<IXmlSerializer>();
            A.CallTo(() => mockXmlSerializerProject1.Deserialize(A<IStringReader>.Ignored)).Returns(fakeProject1);
            A.CallTo(() => mockWrapperFactory.CreateXmlSerializer(typeof(projects1)))
                .Returns(mockXmlSerializerProject1);

            fakeProject1.project.Add(new projectref());
            fakeProject1.project.Add(new projectref());

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

            var factory = new RestFactory(restConfigurations, mockWrapperFactory);

            service = new Service(factory);
        };

        private It should_returne_the_available_project = () =>
        {
            var servers = service.GetServers();
            servers.Count().Should().BeGreaterOrEqualTo(1, "No server retrned!");
            var projects = servers.FirstOrDefault().Projects;
            projects.Count().Should().Be(2, "Wrong count of projects ist returned");
            
            var project = projects.FirstOrDefault();
            project.Name.Should().Be("projectName", "the wrong name is set");
            project.Description.Should().Be("projectDescription", "the wrong description is set");
        }; 
    }
}