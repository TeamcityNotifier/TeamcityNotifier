namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IBuildRepository : IRestObject, INotifyPropertyChanged
    {
        IEnumerable<IBuild> Builds { get; }

        IBuild LastBuild { get; }
    }
}