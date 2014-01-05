namespace TeamcityNotifier.Test
{
    using System.Linq;

    using DataAbstraction;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class BuildDefinitionTest
    {
        private const string Url = "https://teamcity.bbv.ch/buildtype";

        private BuildDefinition testee;

        private buildType buildTypeDto;

        [SetUp]
        public void SetUp()
        {
            this.testee = new BuildDefinition(Url);
            this.buildTypeDto = new buildType
            {
                id = "buildId",
                name = "buildTypeName",
                description = "buildTypeDescprition",
                builds = new buildsref
                         {
                             href = "buildsHref"
                         }
            };
        }

        [Test]
        public void Ctor_WhenBuildTypeIsCreated_UrlIsSet()
        {
            this.testee.Url.Should().NotBeNull();
        }

        [Test]
        public void Ctor_WhenBuildTypeIsCreated_BaseTypeIsSetTobuildType()
        {
            this.testee.BaseType.Should().Be(typeof(buildType));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            this.testee.Id.Should().Be(this.buildTypeDto.id);
        }

        [Test]
        public void SetData_WhenSettingDto_NameIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            this.testee.Name.Should().Be(this.buildTypeDto.name);
        }

        [Test]
        public void SetData_WhenSettingDto_DescriptionIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            this.testee.Description.Should().Be(this.buildTypeDto.description);
        }

        [Test]
        public void SetData_WhenSettingDto_DependenciesAreSet()
        {
            this.testee.SetData(this.buildTypeDto);

            this.testee.Dependencies.Should().NotBeEmpty();
            this.testee.Dependencies.FirstOrDefault().Should().BeAssignableTo<IBuildRepository>();
        }
    }
}
