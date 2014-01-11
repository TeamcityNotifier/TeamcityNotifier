namespace TeamcityNotifier.Test
{
    using System.ComponentModel;

    using FakeItEasy;

    using FluentAssertions;

    using NUnit.Framework;

    using TeamcityNotifier.RestObject;
    using TeamcityNotifier.Wrapper;

    public class ServerTest
    {
        private Server testee;

        private IUri mockUri;

        private IRestConsumer mockRestConsumer;

        [SetUp]
        public void SetUp()
        {
            this.mockUri = A.Fake<IUri>();

            this.mockRestConsumer = A.Fake<IRestConsumer>();

            this.testee = new Server("servername", this.mockUri, mockRestConsumer);
        }

        [Test]
        public void CreateNewServer_WhenParametersAreSet_TheServerNameIsSetCorrectly()
        {
            this.testee.Name.Should().Be("servername", "wrong server name is set");
        }

        [Test]
        public void CreateNewServer_WhenParametersAreSet_TheServerUriIsSetCorrectly()
        {
            this.testee.Uri.Should().Be(mockUri);
        }

        [Test]
        public void CreateNewServer_WhenParametersAreSet_TheRestConsumerIsSetCorrectly()
        {
            this.testee.RestConsumer.Should().Be(mockRestConsumer);
        }

        [Test]
        public void StatusChangedEvent_WhenProjectRepositoryStatusChanged_TheServerStatusIsUpdated()
        {
            this.testee.MonitorEvents();

            var mockProjectRepository = A.Fake<IProjectRepository>();
            A.CallTo(() => mockProjectRepository.Status).Returns(Status.Failure);

            this.testee.ProjectRepository = mockProjectRepository;

            mockProjectRepository.PropertyChanged += Raise.With(new PropertyChangedEventArgs("Status")).Now;

            this.testee.Status.Should().Be(Status.Failure);
        }

        [Test]
        public void StatusChangedEvent_WhenServerStatusIsChanged_TheNotifyPropertyChangedOfStatusIsFired()
        {
            this.testee.MonitorEvents();

            var mockProjectRepository = A.Fake<IProjectRepository>();
            A.CallTo(() => mockProjectRepository.Status).Returns(Status.Failure);

            this.testee.ProjectRepository = mockProjectRepository;

            mockProjectRepository.PropertyChanged += Raise.With(new PropertyChangedEventArgs("Status")).Now;

            this.testee.ShouldRaisePropertyChangeFor(server => server.Status);
        } 
    }
}