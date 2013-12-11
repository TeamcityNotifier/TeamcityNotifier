namespace TeamcityNotifier.Test
{
    using DataAbstraction;

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
            Assert.That(this.testee.Url, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenBuildTypeIsCreated_BaseTypeIsSetTobuildType()
        {
            Assert.That(this.testee.BaseType, Is.EqualTo(typeof(buildType)));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            Assert.That(this.testee.Id, Is.EqualTo(this.buildTypeDto.id));
        }

        [Test]
        public void SetData_WhenSettingDto_NameIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            Assert.That(this.testee.Name, Is.EqualTo(this.buildTypeDto.name));
        }

        [Test]
        public void SetData_WhenSettingDto_DescriptionIsSet()
        {
            this.testee.SetData(this.buildTypeDto);

            Assert.That(this.testee.Description, Is.EqualTo(this.buildTypeDto.description));
        }

        [Test]
        public void SetData_WhenSettingDto_NoDependenciesAreSet()
        {
            this.testee.SetData(this.buildTypeDto);

            Assert.That(this.testee.Dependencies, Is.Empty);
        }
    }
}
