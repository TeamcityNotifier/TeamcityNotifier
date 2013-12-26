namespace TeamcityNotifier
{
    using System;

    using TeamcityNotifier.Wrapper;

    internal class RestConsumer : IRestConsumer
    {
        private readonly IUri baseUrl;
        private readonly IHttpClient httpClient;
        private readonly IWrapperFactory wrapperFactory;

        public RestConsumer(IUri baseUrl, IHttpClient httpClient, IWrapperFactory wrapperFactory)
        {
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
            this.wrapperFactory = wrapperFactory;
        }

        public void Load<T>(T restObject) where T : IRestObject
        {
            this.LoadInstanceData(restObject);
            this.LoadDependencies(restObject);
        }

        private void LoadInstanceData<T>(T restObject) where T : IRestObject
        {
            var uri = wrapperFactory.CreateUri(this.baseUrl, restObject.Url);
            var data = this.Get(restObject.BaseType, uri);
            restObject.SetData(data);
        }

        private void LoadDependencies<T>(T restObject) where T : IRestObject
        {
            foreach (var dependency in restObject.Dependencies)
            {
                this.Load(dependency);
            }
        }

        private object Get(Type baseType, IUri uri)
        {
            var asyncTask = this.httpClient.GetStringAsync(uri);

            var serializer = wrapperFactory.CreateXmlSerializer(baseType);
            var result = asyncTask.Result;

            using (var reader = wrapperFactory.CreateStringReader(result))
            {
                return serializer.Deserialize(reader);
            }
        }
    }
}
