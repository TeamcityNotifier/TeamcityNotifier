namespace TeamcityNotifier.RestObject
{
    using System.Collections.Generic;

    public interface IProject : IRestObject
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool HasParent { get; }
        Status Status { get; }
        IEnumerable<IProject> ChildProjects { get; }
        IEnumerable<IBuildDefinition> BuildDefinitions { get; }
        void AddChild(IProject childProject);
        void RemoveChild(IProject childProject);
    }
}