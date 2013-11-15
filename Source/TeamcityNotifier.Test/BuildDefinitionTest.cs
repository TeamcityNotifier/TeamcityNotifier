namespace TeamcityNotifier.Test
{
    using FluentAssertions;

    using NUnit.Framework;

    using DataAbstraction;

    public class BuildDefinitionTest
    {
        private IBuildDefinition testee;

        private buildType fakeBuildType;

        [SetUp]
        public void SetUp()
        {
            fakeBuildType = new buildType() { name = "buildDefinitionName", description = "buildDefinitionDescription" };

            testee = new BuildDefinition(fakeBuildType);
        }

        [Test]
        public void CreateNewBuildDefinition_WhenParametersAreSet_ThePropertiesAreSetCorrectliy()
        {
            this.testee.Name.Should().Be("buildDefinitionName", "wrong build definition name is set");
            this.testee.Description.Should().Be("buildDefinitionDescription", "wrong build definition description is set");
        }
    }
}