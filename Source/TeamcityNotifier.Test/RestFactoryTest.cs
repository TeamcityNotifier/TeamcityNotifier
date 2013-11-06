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

        private IEnumerable<IRestConfiguration> mockRestConfigurations;

        private IWrapperFactory mockWrapperFactory;

        [SetUp]
        public void SetUp()
        {
            this.mockRestConfigurations = A.Fake<IEnumerable<IRestConfiguration>>();
            this.mockWrapperFactory = A.Fake<IWrapperFactory>();

            //var config1 = A.Fake<IRestConfiguration>();



            this.testee = new RestFactory(this.mockRestConfigurations, this.mockWrapperFactory);
        }

        /*[Test]
        public void CreateServer_WhenServerExists_ThenReturnServer()
        {
            var config1 = A.Fake<IRestConfiguration>();


            var servers = this.testee.CreateServers();

            servers.Count().Should().Be(1,"No server is returned");
        }*/
    }
}