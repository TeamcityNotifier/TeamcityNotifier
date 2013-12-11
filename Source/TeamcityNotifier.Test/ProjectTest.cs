using System.Collections.Generic;

namespace TeamcityNotifier.Test
{
    using System.Linq;

    using DataAbstraction;

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
            Assert.That(this.testee.Url, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenProjectRepositoryIsCreated_BaseTypeIsSetToProjects()
        {
            Assert.That(this.testee.BaseType, Is.EqualTo(typeof(project)));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.Id, Is.EqualTo(this.projectDto.id));
        }

        [Test]
        public void SetData_WhenSettingDto_NameIsSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.Name, Is.EqualTo(this.projectDto.name));
        }

        [Test]
        public void SetData_WhenSettingDto_DescriptionIsSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.Description, Is.EqualTo(this.projectDto.description));
        }

        [Test]
        public void SetData_WhenSettingDto_BuildTypeListIsSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.BuildDefinitions.Count(), Is.EqualTo(this.projectDto.buildTypes.Count));
        }

        [Test]
        public void SetData_WhenSettingDto_BuildTypeDependenciesAreSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.Dependencies, Is.EqualTo(this.testee.BuildDefinitions));
        }

        [Test]
        public void SetData_WhenSettingDto_ParentIdIsSet()
        {
            this.testee.SetData(this.projectDto);

            Assert.That(this.testee.ParentId, Is.EqualTo(projectDto.parentProject.id));
        }

        [Test]
        public void ParentId_WhenParentIdChanges_PropertyChangedIsFiredWithParentIdArgumentIsRaised()
        {
            var eventRaised = false;

            this.testee.PropertyChanged += (sender, args) => eventRaised = true;

            this.testee.SetData(this.projectDto);

            Assert.IsTrue(eventRaised);
        }


        [Test]
        public void AddChild_WhenAddingChildProject_ChildIsAddedToTheList()
        {
            var childProject = new Project("childProjectUrl");

            this.testee.AddChild(childProject);

            Assert.That(this.testee.ChildProjects, Contains.Item(childProject));
        }

        [Test]
        public void RemoveChild_WhenRemovingChildProject_ChildIsRemovedFromTheList()
        {
            var childProject = new Project("childProjectUrl");

            this.testee.AddChild(childProject);

            this.testee.RemoveChild(childProject);

            Assert.That(this.testee.ChildProjects, Is.Empty);
        }

    }
}
