﻿namespace TeamcityNotifier.RestObject
{
    public interface IBuildDefinition : IRestObject
    {
        string Name { get; }
        string Description { get; }
        string Id { get; }
        Status Status { get; }
        IBuildRepository BuildRepository { get; }
    }
}