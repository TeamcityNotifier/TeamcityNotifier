namespace TeamcityNotifier
{
    using TeamcityNotifier.Wrapper;

    public interface INetworkFactory
    {
        IRestConsumer CreateRestConsumer(IUri baseUri, IHttpClient httpClientHandler, IWrapperFactory wrapperFactory);

        IUpdater CreateUpdater(IRestConsumer restConsumer);
    }
}