namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IBuildRepository : IRestObject
    {
        IEnumerable<IBuild> Builds { get; }

        IBuild LastBuild { get; }
    }
}