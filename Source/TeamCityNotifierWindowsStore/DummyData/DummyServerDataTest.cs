namespace TeamcityNotifier.Test
{
    using System.Linq;

    using NUnit.Framework;

    using TeamCityNotifierWindowsStore.DummyData;

    public class ServiceTest
    {
        private DummyServerData testee;

        [Test]
        public void SetupDummyServerData_CheckIfDataAreCorrect()
        {
            this.testee = new DummyServerData();
            
            Assert.AreEqual(this.testee.AllGroups.Count, 1);
            Assert.AreEqual(this.testee.AllGroups.First().RootProject.ChildProjects.Count, 3);
            Assert.AreEqual(this.testee.AllGroups.First().RootProject.ChildProjects.First().ChildProjects.Count, 1);
        }
    }
}