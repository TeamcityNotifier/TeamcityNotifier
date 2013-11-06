namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier;
    using TeamcityNotifier.Wrapper;

    public class RestFactoryTest
    {
        private IFactory testee;

        private IList<IRestConfiguration> restConfigurations;

        private IWrapperFactory mockWrapperFactory;

        [SetUp]
        public void SetUp()
        {
            this.restConfigurations = new List<IRestConfiguration>();
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();

            var config1 = A.Fake<IRestConfiguration>();
            A.CallTo(() => config1.BaseUrl).Returns("url1");
            A.CallTo(() => config1.UserName).Returns("user1");
            A.CallTo(() => config1.Password).Returns("password1");
            restConfigurations.Add(config1);


            var config2 = A.Fake<IRestConfiguration>();
            A.CallTo(() => config2.BaseUrl).Returns("url2");
            A.CallTo(() => config2.UserName).Returns("user2");
            A.CallTo(() => config2.Password).Returns("password2");
            restConfigurations.Add(config2);
            

            this.testee = new RestFactory(this.restConfigurations, this.mockWrapperFactory);
        }

        [Test]
        public void CreateServer_WhenTwoServersAreConfigured_ThenReturnTwoServers()
        {
            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(2,"No server is returned");
        }

        /* private class HttpClientStub : HttpClient
        {
            
        }


        var asyncTask = httpClient.GetStringAsync(url);

            var serializer = this.wrapperFactory.CreateXmlSerializer(typeof(T));

            using (var reader = this.wrapperFactory.CreateStringReader(asyncTask.Result))
            {
                var objectDto = (T)serializer.Deserialize(reader);

                return objectDto;
            }*/
    }
}