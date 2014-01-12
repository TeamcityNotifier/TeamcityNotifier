namespace TeamcityNotifier.RestObject
{
    using System;

    public interface IBuildDefinition : IRestObject
    {
        Guid UniqueId { get; }
        string Name { get; }
        string Description { get; }
        string Id { get; }
        Status Status { get; }
        IBuildRepository BuildRepository { get; }
    }
}