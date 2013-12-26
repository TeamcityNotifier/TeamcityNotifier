﻿namespace TeamcityNotifier.Test
{
    using DataAbstraction;

    using FluentAssertions;

    using NUnit.Framework;

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
            Assert.That(this.testee.Url, Is.Not.Null);
        }

        [Test]
        public void Ctor_WhenBuildIsCreated_BaseTypeIsSetToBuild()
        {
            Assert.That(this.testee.BaseType, Is.EqualTo(typeof(build)));
        }

        [Test]
        public void SetData_WhenSettingDto_IdIsSet()
        {
            this.testee.SetData(this.buildDto);

            Assert.That(this.testee.Id, Is.EqualTo(this.buildDto.id));
        }

        [Test]
        public void SetData_WhenSettingDto_NumberIsSet()
        {
            this.testee.SetData(this.buildDto);

            Assert.That(this.testee.Number, Is.EqualTo(this.buildDto.number));
        }

        [Test]
        public void SetData_WhenSettingDto_StartDateIsSet()
        {
            this.testee.SetData(this.buildDto);

            Assert.That(this.testee.StartDate, Is.EqualTo(this.buildDto.startDate));
        }

        [Test]
        public void SetData_WhenSettingDto_FinishDateIsSet()
        {
            this.testee.SetData(this.buildDto);

            Assert.That(this.testee.FinishDate, Is.EqualTo(this.buildDto.finishDate));
        }

        [Test]
        public void SetData_WhenSettingDto_NoDependenciesAreSet()
        {
            this.testee.SetData(this.buildDto);

            Assert.That(this.testee.Dependencies, Is.Empty);
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