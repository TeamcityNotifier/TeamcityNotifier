namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IProject : IRestObject, INotifyPropertyChanged
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