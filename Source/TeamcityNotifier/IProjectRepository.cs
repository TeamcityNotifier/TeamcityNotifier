namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IProjectRepository : IRestObject
    {
        IEnumerable<IProject> Projects { get; }
    }
}