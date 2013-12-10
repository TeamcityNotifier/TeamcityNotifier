namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    public interface IRestObject
    {
        string Url { get; }

        Type BaseType { get; }

        IEnumerable<IRestObject> Dependencies { get;  }

        void SetData(object obj);
    }
}