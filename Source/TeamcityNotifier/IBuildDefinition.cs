namespace TeamcityNotifier
{
    public interface IBuildDefinition : ILocalizable
    {
        string Name { get; }
        string Description { get; }
    }
}