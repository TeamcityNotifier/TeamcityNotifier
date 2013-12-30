namespace TeamcityNotifier
{
    using System.ComponentModel;

    public interface IBuildDefinition : IRestObject, INotifyPropertyChanged
    {
        string Name { get; }
        string Description { get; }
        string Id { get; }
        Status Status { get; }
        IBuildRepository BuildRepository { get; }
        IBuild LastBuild { get;  set; }
    }
}