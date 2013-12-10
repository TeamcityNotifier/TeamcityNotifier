namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IBuildRepository : IRestObject
    {
        IEnumerable<IBuild> Builds { get; }
    }
}