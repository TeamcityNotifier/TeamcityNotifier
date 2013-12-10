namespace TeamcityNotifier
{
    public interface IBuild : IRestObject
    {
        long Id { get; set; }
        string Status { get; }
        string Number { get; }
        string FinishDate { get; set; }
        string StartDate { get; set; }
    }
}