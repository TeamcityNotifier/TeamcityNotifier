namespace TeamcityNotifier
{
    using System.ComponentModel;

    public interface IBuildDefinition : IRestObject, INotifyPropertyChanged
    {
        string Name { get; }
        string Description { get; }
        string Id { get; }
        string BuildRepositoryUrl { get; }
        IBuild LastBuild { get;  set; }
    }
}