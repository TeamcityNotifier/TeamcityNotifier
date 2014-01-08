namespace TeamcityNotifier
{
    public interface IUpdater
    {
        void Register(IRestObject restObject);

        void Start();
    }
}