namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IRestObject : INotifyPropertyChanged
    {
        string Url { get; }

        Type BaseType { get; }

        IEnumerable<IRestObject> Dependencies { get;  }

        void SetData(object obj);
    }
}