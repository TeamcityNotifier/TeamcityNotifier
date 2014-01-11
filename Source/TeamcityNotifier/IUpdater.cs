namespace TeamcityNotifier
{
    using TeamcityNotifier.RestObject;

    public interface IUpdater
    {
        void Register(IRestObject restObject);

        void Start();

        void Stop();
    }
}