namespace TeamcityNotifier
{
    public interface IBuildDefinition : IRestObject
    {
        string Name { get; }
        string Description { get; }
        string Id { get; }
        string BuildRepositoryUrl { get; }
    }
}