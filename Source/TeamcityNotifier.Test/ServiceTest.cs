namespace TeamcityNotifier.Test
{
    using FakeItEasy;

    using NUnit.Framework;

    public class ServiceTest
    {
        private IService testee;

        private IRestFactory mockRestFactory;

        [SetUp]
        public void SetUp()
        {
            this.mockRestFactory = A.Fake<IRestFactory>();

            this.testee = new Service(this.mockRestFactory);
        }

        [Test]
        public void Test()
        {
            this.testee.GetServers();

            A.CallTo(()=> this.mockRestFactory.CreateServers()).MustHaveHappened();
        }

    }
}