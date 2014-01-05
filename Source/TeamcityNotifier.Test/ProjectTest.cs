using System.Collections.Generic;

namespace TeamcityNotifier.Test
{
    using System.Linq;

    using DataAbstraction;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectTest
    {
        private const string Url = "https://teamcity.bbv.ch/project";

        private Project testee;

        private project projectDto;

        [SetUp]
        public void SetUp()
        {
            this.testee = new Project(Url);
            this.projectDto = new project
            {
                id = "projectId",
                name = "projectName",
                description = "projectDescprition",
                parentProject = new projectref { id = "parentProjectId" },
                buildTypes = new List<buildTyperef>
                             {
                                 new buildTyperef
                                 {
                                     href = "buildtype1Href"
                                 },
                                 new buildTyperef
                                 {
                                     href = "buildtype2Href"
                                 }
                             }
            };
        }

        [Test]
        public void Ctor_WhenProjectRepositoryIsCreated_UrlIsSet()
        {
            this.testee.Url.Should().NotBeNull();
        }

        [Test]
        public void Ctor_WhenProjectRepositoryIsCreated_BaseTypeIsSetToProjects()
        {
            this.testee.BaseType.Should().Be(typeof(project));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.Id.Should().Be(this.projectDto.id);
        }

        [Test]
        public void SetData_WhenSettingDto_NameIsSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.Name.Should().Be(this.projectDto.name);
        }

        [Test]
        public void SetData_WhenSettingDto_DescriptionIsSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.Description.Should().Be(this.projectDto.description);
        }

        [Test]
        public void SetData_WhenSettingDto_BuildTypeListIsSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.BuildDefinitions.Count().Should().Be(this.projectDto.buildTypes.Count);
        }

        [Test]
        public void SetData_WhenSettingDto_BuildTypeDependenciesAreSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.Dependencies.Should().BeEquivalentTo(this.testee.BuildDefinitions);
        }

        [Test]
        public void SetData_WhenSettingDto_ParentIdIsSet()
        {
            this.testee.SetData(this.projectDto);

            this.testee.ParentId.Should().Be(projectDto.parentProject.id);
        }

        [Test]
        public void SetData_WhenParentIdIsSet_HasParent()
        {
            this.testee.SetData(this.projectDto);

            this.testee.HasParent.Should().BeTrue();
        }

        [Test]
        public void SetData_WhenParentIdIsNotSet_HasNoParent()
        {
            this.projectDto.parentProject.id = string.Empty;

            this.testee.SetData(this.projectDto);

            this.testee.HasParent.Should().BeFalse();
        }

        [Test]
        public void ParentId_WhenParentIdChanges_PropertyChangedIsFiredWithParentIdArgumentIsRaised()
        {
            this.testee.MonitorEvents();

            this.testee.SetData(this.projectDto);

            this.testee.ShouldRaisePropertyChangeFor(project => project.ParentId);
        }


        [Test]
        public void AddChild_WhenAddingChildProject_ChildIsAddedToTheList()
        {
            var childProject = new Project("childProjectUrl");

            this.testee.AddChild(childProject);

            this.testee.ChildProjects.Should().Contain(childProject);
        }

        [Test]
        public void RemoveChild_WhenRemovingChildProject_ChildIsRemovedFromTheList()
        {
            var childProject = new Project("childProjectUrl");

            this.testee.AddChild(childProject);

            this.testee.RemoveChild(childProject);

            this.testee.ChildProjects.Should().BeEmpty();
        }

    }
}
