namespace TeamcityNotifier.RestObject
{
    using System.Collections.Generic;

    public interface IBuildRepository : IRestObject
    {
        IEnumerable<IBuild> Builds { get; }

        IBuild LastBuild { get; }
    }
}