namespace TeamcityNotifier.Wrapper
{
    using System;
    using System.IO;

    public interface IStringReader : IDisposable
    {
        StringReader ToStringReader();
    }
}