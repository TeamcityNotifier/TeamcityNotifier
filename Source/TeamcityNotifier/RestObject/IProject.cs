namespace TeamcityNotifier.RestObject
{
    using System;
    using System.Collections.ObjectModel;

    public interface IProject : IRestObject
    {
        Guid UniqueId { get; }
        string Id { get; }
        string Name { get; }
        string Description { get; }
        bool HasParent { get; }
        object Parent { get; set; }
        Status Status { get; }
        ObservableCollection<IProject> ChildProjects { get; }
        ObservableCollection<IBuildDefinition> BuildDefinitions { get; }
        void AddChild(IProject childProject);
        void RemoveChild(IProject childProject);
    }
}