namespace TeamcityNotifier.Specification
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using Machine.Specifications;

    using TeamcityNotifier.Wrapper;

    using DataAbstraction;

    public class When_User_Shows_Servers
    {
        private static IService service;


        private Establish context = () =>
            {
                var mockWrapperFactory = A.Fake<IWrapperFactory>();

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

                fakeProject1.project.Add(new projectref());
                fakeProject1.project.Add(new projectref());

                var restConfigurations = new List<IRestConfiguration>();

                var configuration = A.Fake<IRestConfiguration>();
                A.CallTo(() => configuration.BaseUrl).Returns("url1");
                A.CallTo(() => configuration.UserName).Returns("user1");
                A.CallTo(() => configuration.Password).Returns("password1");
                A.CallTo(() => configuration.Name).Returns("serverName1");
                restConfigurations.Add(configuration);


                configuration = A.Fake<IRestConfiguration>();
                A.CallTo(() => configuration.BaseUrl).Returns("url2");
                A.CallTo(() => configuration.UserName).Returns("user2");
                A.CallTo(() => configuration.Password).Returns("password2");
                A.CallTo(() => configuration.Name).Returns("serverName2");
                restConfigurations.Add(configuration);

                var factory = new RestFactory(restConfigurations, mockWrapperFactory);

                service = new Service(factory);
            };

        private It should_returne_the_configured_servers = () =>
    {
        var servers = service.GetServers();
        servers.Count().Should().Be(2, "Wrong count of servers is returned!");
        var server = servers.FirstOrDefault();
        server.UserName.Should().Be("user1", "Wrong username is configured");
        server.Password.Should().Be("password1", "Wrong password is configured");
        server.Name.Should().Be("serverName1", "Wrong server name is configured");
    };
    }
}