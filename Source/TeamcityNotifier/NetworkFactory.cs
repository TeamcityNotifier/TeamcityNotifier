namespace TeamcityNotifier
{
    using TeamcityNotifier.Wrapper;

    public class NetworkFactory : INetworkFactory
    {
        public IRestConsumer CreateRestConsumer(IUri baseUri, IHttpClient httpClientHandler, IWrapperFactory wrapperFactory)
        {
            return new RestConsumer(baseUri, httpClientHandler, wrapperFactory);
        }

        public IUpdater CreateUpdater(IRestConsumer restConsumer)
        {
            return new Updater(restConsumer);
        }
    }
}