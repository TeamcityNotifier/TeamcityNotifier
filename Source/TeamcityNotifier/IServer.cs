namespace TeamCityRestClient
{
    using System.Collections.Generic;

    public interface IServer
    {
        IEnumerable<IProject> GetProjects();
    }
}