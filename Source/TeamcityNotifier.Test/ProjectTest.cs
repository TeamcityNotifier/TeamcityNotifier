namespace TeamcityNotifier.Test
{
    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using xsdtest;

    public class ProjectTest
    {
        private IProject testee;

        private project fakeProject;


        [SetUp]
        public void SetUp()
        {
            fakeProject = new project(){name = "projectName", description = "projectDescription"};

            this.testee = new Project(fakeProject);
        }

        [Test]
        public void CreateNewProject_WhenParametersAreSet_ThePropertiesAreSetCorrectliy()
        {
            this.testee.Name.Should().Be("projectName", "wrong project name is set");
            this.testee.Description.Should().Be("projectDescription", "wrong project description is set");
        }
    }
}