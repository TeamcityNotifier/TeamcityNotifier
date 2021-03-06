﻿namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAbstraction;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier.RestObject;

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
            this.testee.Url.Should().NotBeNull();
        }

        [Test]
        public void Ctor_WhenBuildRepositoryIsCreated_BaseTypeIsSetToBuilds()
        {
            testee.BaseType.Should().Be(typeof(builds));
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

            testee.Builds.Count().Should().Be(2);
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

            testee.Dependencies.Count().Should().Be(2);
        }
    }
}
