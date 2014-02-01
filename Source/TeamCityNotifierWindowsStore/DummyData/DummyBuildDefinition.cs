using System;
using System.Collections.Generic;

namespace TeamCityNotifierWindowsStore.DummyData
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using TeamcityNotifier;
    using TeamcityNotifier.RestObject;

    public class DummyBuildDefinition : IBuildDefinition
    {
        public DummyBuildDefinition()
        {
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

        public string Name { get; set; }

        public string Description { get; set; }

        public string Id { get; private set; }

        public Status Status { get; set; }

        public IBuildRepository BuildRepository { get; private set; }
    }
}
