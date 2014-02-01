namespace TeamCityNotifierWindowsStore.DummyData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;

    public class DummyProject : IProject
    {
        public DummyProject()
        {
            ChildProjects = new ObservableCollection<IProject>();
            BuildDefinitions = new ObservableCollection<IBuildDefinition>();
            Dependencies = new ObservableCollection<IRestObject>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Url { get; private set; }

        public Type BaseType { get; private set; }

        public IEnumerable<IRestObject> Dependencies { get; private set; }

        public void SetData(object obj)
        {
        }

        public Guid UniqueId { get; private set; }

        public string Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasParent { get; private set; }

        public object Parent { get; set; }

        public Status Status { get; set; }

        public ObservableCollection<IProject> ChildProjects { get; set; }

        public ObservableCollection<IBuildDefinition> BuildDefinitions { get; set; }

        public void AddChild(IProject childProject)
        {
            ChildProjects.Add(childProject);
        }

        public void RemoveChild(IProject childProject)
        {
            ChildProjects.Remove(childProject);
        }
    }
}