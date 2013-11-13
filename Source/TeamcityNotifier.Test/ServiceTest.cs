namespace TeamcityNotifier.Test
{
    using FakeItEasy;

    using NUnit.Framework;

    public class ServiceTest
    {
        private IService testee;

        private IFactory mockFactory;

        [SetUp]
        public void SetUp()
        {
            this.mockFactory = A.Fake<IFactory>();

            this.testee = new Service(this.mockFactory);
        }

        [Test]
        public void Test()
        {
            this.testee.GetServers();

            A.CallTo(()=> this.mockFactory.CreateServers()).MustHaveHappened();
        }

    }
}