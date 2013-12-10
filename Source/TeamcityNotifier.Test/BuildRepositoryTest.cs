namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAbstraction;

    using NUnit.Framework;

    [TestFixture]
    public class BuildRepositoryTest
    {
        private const string Url = "https://teamcity.bbv.ch/builds";

        private BuildRepository testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new BuildRepository(Url);
        }

        [Test]
        public void Ctor_WhenBuildRepositoryIsCreated_UrlIsSet()
        {
            Assert.That(testee.Url, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenBuildRepositoryIsCreated_BaseTypeIsSetToBuilds()
        {
            Assert.That(testee.BaseType, Is.EqualTo(typeof(builds)));
        }

        [Test]
        public void SetData_WhenSettingDtoWithBuilds_BuildsContainsNewBuilds()
        {
            var buildRefList = new List<buildref>
            {
                new buildref{href = "project1Href"},
                new buildref{href = "project2Href"}
            };

            var buildsDto = new builds
            {
                build = buildRefList 
            };

            this.testee.SetData(buildsDto);

            Assert.That(testee.Builds.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SetData_WhenSettingDtoWithProjects_TheseProjectsAreReturnedAsDependencies()
        {
            var buildRefList = new List<buildref>
            {
                new buildref{href = "project1Href"},
                new buildref{href = "project2Href"}
            };

            var buildsDto = new builds
            {
                build = buildRefList
            };

            this.testee.SetData(buildsDto);

            Assert.That(testee.Dependencies.Count(), Is.EqualTo(2));
        }
    }
}
