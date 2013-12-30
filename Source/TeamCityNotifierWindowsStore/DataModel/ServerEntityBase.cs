namespace TeamCityNotifierWindowsStore.DataModel
{
    using System;

    using TeamcityNotifier;

    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Base class for <see cref="ProjectPMod"/> and <see cref="ServerPMod"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class ServerEntityBase : Common.BindableBase
    {
        private const string PathFailedPicture = "Assets/Red.png";

        private const string PathSuccessfulPicture = "Assets/Green.png";

        private static readonly Uri BaseUri = new Uri("ms-appx:///");

        private string uniqueId = string.Empty;

        private string title = string.Empty;

        private string description = string.Empty;

        private Status status;

        private ImageSource image;

        private string imagePath;

        protected ServerEntityBase(string uniqueId, string title, string description, Status status)
        {
            this.uniqueId = uniqueId;
            this.title = title;
            this.description = description;
            this.status = status;
        }

        public string UniqueId
        {
            get { return this.uniqueId; }
            set { this.SetProperty(ref this.uniqueId, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        public Status Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.SetProperty(ref this.status, value);
                if (value == Status.Success)
                {
                    this.SetImage(PathSuccessfulPicture);
                }
                else
                {
                    this.SetImage(PathFailedPicture);
                }
            }
        }

        public ImageSource Image
        {
            get
            {
                if (this.image == null && this.imagePath != null)
                {
                    this.image = new BitmapImage(new Uri(BaseUri, this.imagePath));
                }
                return this.image;
            }

            set
            {
                this.imagePath = null;
                this.SetProperty(ref this.image, value);
            }
        }

        public void SetImage(string path)
        {
            this.image = null;
            this.imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}