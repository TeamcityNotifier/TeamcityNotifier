namespace TeamcityNotifier
{
    public interface IRestConsumer
    {
        void Load<T>(T restObject) where T : IRestObject;
    }
}