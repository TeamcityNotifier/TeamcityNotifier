namespace TeamcityNotifier
{
    public interface IRestConfiguration
    {
        string BaseUrl { get; }

        string UserName { get; }

        string Password { get; }

        string Name { get; }
    }
}