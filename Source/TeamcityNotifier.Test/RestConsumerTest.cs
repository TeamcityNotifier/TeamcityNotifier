namespace TeamcityNotifier.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FakeItEasy;

    using NUnit.Framework;

    using TeamcityNotifier.Wrapper;

    [TestFixture]
    public class RestConsumerTest
    {
        private const string BaseUrl = "https://teamcity.bbv.ch/";

        private RestConsumer testee;

        private IHttpClient fakeHttpClient;

        private IWrapperFactory fakeFactory;

        private IUri fakeUri;

        [SetUp]
        public void SetUp()
        {
            var fakeTask = new Task<string>(() => "<FakeSerializableObject></FakeSerializableObject>");

            this.fakeUri = A.Fake<IUri>();

            A.CallTo(() => fakeUri.ToUri()).Returns(new Uri(BaseUrl));

            this.fakeFactory = A.Fake<IWrapperFactory>();

            this.fakeHttpClient = A.Fake<IHttpClient>();
            A.CallTo(() => this.fakeHttpClient.GetStringAsync(A<IUri>.Ignored)).Returns(fakeTask);
            this.fakeHttpClient.GetStringAsync(fakeUri).Start();

            this.testee = new RestConsumer(fakeUri, this.fakeHttpClient, fakeFactory);
        }

        [Test]
        public void Load_WhenLoadingARestObject_CorrectUrlIsUsed()
        {
            const string RelativeUri = "relativeUri";

            var fakeRestObject = A.Fake<IRestObject>();
            A.CallTo(() => fakeRestObject.Url).Returns(RelativeUri);
            A.CallTo(() => fakeRestObject.BaseType).Returns(typeof(FakeSerializableObject));
            A.CallTo(() => fakeFactory.CreateUri(A<IUri>.Ignored, A<string>.Ignored)).Returns(fakeUri);

            this.testee.Load(fakeRestObject);

            A.CallTo(() => this.fakeHttpClient.GetStringAsync(fakeUri)).MustHaveHappened();
        }

        [Test]
        public void Load_WhenLoadingARestObject_LoadedDataAreSet()
        {
            const string RelativeUri = "relativeUri";
            var fakeSerializer = A.Fake<IXmlSerializer>();
            var fakeRestObject = A.Fake<IRestObject>();
            var fakeDeserializedObject = new FakeSerializableObject();
            A.CallTo(() => fakeRestObject.Url).Returns(RelativeUri);
            A.CallTo(() => fakeRestObject.BaseType).Returns(typeof(FakeSerializableObject));
            A.CallTo(() => fakeFactory.CreateXmlSerializer(A<Type>.Ignored)).Returns(fakeSerializer);
            A.CallTo(() => fakeSerializer.Deserialize(A<IStringReader>.Ignored)).Returns(fakeDeserializedObject);

            this.testee.Load(fakeRestObject);

            A.CallTo(() => fakeRestObject.SetData(fakeDeserializedObject)).MustHaveHappened();
        }

        [Test]
        [Ignore("Null pointer in Assert!?!")]
        public void Load_WhenLoadingARestObject_DependenciesAreLoaded()
        {
            const string RelativeUri = "relativeUri";
            const string RelativeUri2 = "relativeUri2";

            var fakeRestDependency = A.Fake<IRestObject>();
            A.CallTo(() => fakeRestDependency.Url).Returns(RelativeUri2);
            A.CallTo(() => fakeRestDependency.BaseType).Returns(typeof(FakeSerializableObject));

            IEnumerable<IRestObject> dependencies = new List<IRestObject> { fakeRestDependency };

            var fakeRestObject = A.Fake<IRestObject>();
            A.CallTo(() => fakeRestObject.Url).Returns(RelativeUri);
            A.CallTo(() => fakeRestObject.BaseType).Returns(typeof(FakeSerializableObject));
            A.CallTo(() => fakeRestObject.Dependencies).Returns(dependencies);

            this.testee.Load(fakeRestObject);

            A.CallTo(() => this.testee.Load<IRestObject>(fakeRestDependency)).MustHaveHappened();
        }

        public class FakeSerializableObject
        {
            
        }

    }
}
