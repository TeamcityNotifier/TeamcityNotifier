namespace TeamcityNotifier.RestObject
{
    using System.Collections.Generic;

    public interface IProjectRepository : IRestObject
    {
        IEnumerable<IProject> Projects { get; }

        Status Status { get; set; }
    }
}