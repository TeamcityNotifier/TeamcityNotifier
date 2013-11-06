namespace TeamcityNotifier.Wrapper
{
    using System;

    public interface IWrapperFactory
    {
        IXmlSerializer CreateXmlSerializer(Type type);

        IHttpClient CreateHttpClientHandler(string username, string password);

        IStringReader CreateStringReader(string str);

        IUri CreateUri(string baseUri);

        IUri CreateUri(IUri baseUri, string relativeUri);
    }
}