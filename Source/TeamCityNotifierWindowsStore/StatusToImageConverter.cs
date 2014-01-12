namespace TeamCityNotifierWindowsStore
{
    using System;

    using TeamcityNotifier;

    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Imaging;

    public class StatusToImageConverter : IValueConverter
    {
        private static readonly Uri BaseUri = new Uri("ms-appx:///");

        private const string PathFailedPicture = "Assets/Red.png";

        private const string PathSuccessfulPicture = "Assets/Green.png";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((Status)value == Status.Success)
            {
                return new BitmapImage(new Uri(BaseUri, PathSuccessfulPicture));
            }
            else
            {
                return new BitmapImage(new Uri(BaseUri, PathFailedPicture));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}