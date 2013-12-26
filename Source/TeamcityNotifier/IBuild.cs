namespace TeamcityNotifier
{
    using System.ComponentModel;

    public interface IBuild : IRestObject, INotifyPropertyChanged
    {
        long Id { get; set; }
        Status Status { get; }
        string Number { get; }
        string FinishDate { get; set; }
        string StartDate { get; set; }
    }
}