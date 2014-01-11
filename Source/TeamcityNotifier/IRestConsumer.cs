namespace TeamcityNotifier
{
    using TeamcityNotifier.RestObject;

    public interface IRestConsumer
    {
        void Load<T>(T restObject) where T : IRestObject;
    }
}