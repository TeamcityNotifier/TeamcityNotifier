﻿namespace TeamcityNotifier.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAbstraction;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectRepositoryTest
    {
        private const string Url = "https://teamcity.bbv.ch/projects";

        private ProjectRepository testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new ProjectRepository(Url);
        }

        [Test]
        public void Ctor_WhenProjectRepositoryIsCreated_UrlIsSet()
        {
            Assert.That(testee.Url, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenProjectRepositoryIsCreated_BaseTypeIsSetToProjects()
        {
            Assert.That(testee.BaseType, Is.EqualTo(typeof(projects1)));
        }

        [Test]
        public void SetData_WhenSettingDtoWithProjects_AllProjectsContainsNewProjects()
        {
            var projectRefList = new List<projectref>
            {
                new projectref{href = "project1Href"},
                new projectref{href = "project2Href"}
            };

            var projects1Dto = new projects1
            {
                project = projectRefList
            };

            this.testee.SetData(projects1Dto);

            Assert.That(testee.Projects.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SetData_WhenSettingDtoWithProjects_TheseProjectsAreReturnedAsDependencies()
        {
            var projectRefList = new List<projectref>
            {
                new projectref{href = "project1Href"},
                new projectref{href = "project2Href"}
            };

            var projects1Dto = new projects1
            {
                project = projectRefList
            };

            this.testee.SetData(projects1Dto);

            Assert.That(testee.Dependencies.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SetData_WhenSettingDtoWithProjects_CorrectProjectInstanceWithHrefIsCreated()
        {
            const string project1href = "project1Href";

            var projectRefList = new List<projectref>
            {
                new projectref{href = project1href}
            };

            var projects1Dto = new projects1
            {
                project = projectRefList
            };

            this.testee.SetData(projects1Dto);

            Assert.That(testee.Projects.First().Url, Is.EqualTo(project1href));
        }

        [Test]
        public void ProjectPropertyChanged_WhenParentIdOfAProjectChanges_RepositoryAddChangedProjectToCorrectParentsChildList()
        {
            var parentProjectRef = new projectref { href = "projectParentHref" };
            var childProjectRef = new projectref { href = "projectChildHref" };

            var projectRefList = new List<projectref>
            {
                parentProjectRef,
                childProjectRef
            };

            var projects1Dto = new projects1
            {
                project = projectRefList
            };

            var parentProjectDto = new project { id = "parentProject", parentProject = new projectref (), buildTypes = new List<buildTyperef>()};
            var childProjectDto = new project { id = "childProject", parentProject = new projectref { id = "parentProject" },  buildTypes = new List<buildTyperef>()};

            this.testee.SetData(projects1Dto);

            var parentProject =  this.testee.Projects.First(project => project.Url == parentProjectRef.href);
            var childProject = this.testee.Projects.First(project => project.Url == childProjectRef.href);

            parentProject.SetData(parentProjectDto);
            childProject.SetData(childProjectDto);

            Assert.That(this.testee.Projects.First(project => project.Url == parentProject.Url).ChildProjects, Contains.Item(childProject));
        }

        [Test]
        public void ProjectPropertyChanged_WhenParentIdOfAProjectChanges_RepositoryRemovesChangedProjectFromParentsChildList()
        {
            var parentProjectRef = new projectref { href = "projectParentHref" };
            var secondParentProjectRef = new projectref { href = "secondProjectParentHref" };
            var childProjectRef = new projectref { href = "projectChildHref" };

            var projectRefList = new List<projectref>
            {
                parentProjectRef,
                secondParentProjectRef,
                childProjectRef
            };

            var projects1Dto = new projects1
            {
                project = projectRefList
            };

            var parentProjectDto = new project { id = "parentProject", parentProject = new projectref(), buildTypes = new List<buildTyperef>() };
            var secondParentProjectDto = new project { id = "secondParentProject", parentProject = new projectref(), buildTypes = new List<buildTyperef>() };
            var childProjectDto = new project { id = "childProject", parentProject = new projectref { id = "parentProject" }, buildTypes = new List<buildTyperef>() };
            var changedParentchildProjectDto = new project { id = "childProject", parentProject = new projectref { id = "secondParentProject" }, buildTypes = new List<buildTyperef>() };

            this.testee.SetData(projects1Dto);

            var parentProject = this.testee.Projects.First(project => project.Url == parentProjectRef.href);
            var secondParentProject = this.testee.Projects.First(project => project.Url == secondParentProjectRef.href);
            var childProject = this.testee.Projects.First(project => project.Url == childProjectRef.href);

            parentProject.SetData(parentProjectDto);
            secondParentProject.SetData(secondParentProjectDto);
            childProject.SetData(childProjectDto);

            childProject.SetData(changedParentchildProjectDto);

            Assert.That(this.testee.Projects.First(project => project.Url == parentProject.Url).ChildProjects, Is.Empty);
        }

    }
}