namespace TeamcityNotifier.Test
{
    using DataAbstraction;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier.RestObject;

    [TestFixture]
    public class BuildTest
    {
        private const string Url = "https://teamcity.bbv.ch/build";

        private Build testee;

        private build buildDto;

        [SetUp]
        public void SetUp()
        {
            this.testee = new Build(Url);
            this.buildDto = new build
            {
                id = 1,
                number = "buildNumber",
                status = "buildStatus",
                startDate = "buildStartDate",
                finishDate = "buildfinishDate",
            };
        }

        [Test]
        public void Ctor_WhenBuildIsCreated_UrlIsSet()
        {
            this.testee.Url.Should().NotBeNull();
        }

        [Test]
        public void Ctor_WhenBuildIsCreated_BaseTypeIsSetToBuild()
        {
            this.testee.BaseType.Should().Be(typeof(build));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.buildDto);

            this.testee.Id.Should().Be(this.buildDto.id);
        }

        [Test]
        public void SetData_WhenSettingDto_NumberIsSet()
        {
            this.testee.SetData(this.buildDto);

            this.testee.Number.Should().Be(this.buildDto.number);
        }

        [Test]
        public void SetData_WhenSettingDto_StartDateIsSet()
        {
            this.testee.SetData(this.buildDto);

            this.testee.StartDate.Should().Be(this.buildDto.startDate);
        }

        [Test]
        public void SetData_WhenSettingDto_FinishDateIsSet()
        {
            this.testee.SetData(this.buildDto);

            this.testee.FinishDate.Should().Be(this.buildDto.finishDate);
        }

        [Test]
        public void SetData_WhenSettingDto_NoDependenciesAreSet()
        {
            this.testee.SetData(this.buildDto);

            this.testee.Dependencies.Should().BeEmpty();
        }

        [Test]
        public void SetData_WhenSettingDtoStatusToError_StatusIdSetToError()
        {
            this.buildDto.status = "ERROR";
            this.testee.SetData(this.buildDto);

            this.testee.Status.Should().Be(Status.Error);
        }

        [Test]
        public void SetData_WhenSettingDtoStatusToFailure_StatusIdSetToFailure()
        {
            this.buildDto.status = "FAILURE";
            this.testee.SetData(this.buildDto);

            this.testee.Status.Should().Be(Status.Failure);
        }

        [Test]
        public void SetData_WhenSettingDtoStatusToSuccess_StatusIdSetToSuccess()
        {
            this.buildDto.status = "SUCCESS";
            this.testee.SetData(this.buildDto);

            this.testee.Status.Should().Be(Status.Success);
        }
    }
}
