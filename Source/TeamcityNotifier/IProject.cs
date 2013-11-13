namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public interface IProject : ILocalizable
    {
        string Name { get; }
        string Description { get; }
        IEnumerable<IBuildDefinition> BuildDefinitions { get; }
    }
}