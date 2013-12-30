namespace TeamCityNotifierWindowsStore.DataModel
{
    using System;

    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Base class for <see cref="ProjectPMod"/> and <see cref="ServerPMod"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class ServerEntityBase : Common.BindableBase
    {
        private static readonly Uri baseUri = new Uri("ms-appx:///");

        protected ServerEntityBase(string uniqueId, string title, string imagePath, string description)
        {
            this.uniqueId = uniqueId;
            this.title = title;
            this.description = description;
            this.imagePath = imagePath;
        }

        private string uniqueId = string.Empty;

        public string UniqueId
        {
            get { return this.uniqueId; }
            set { this.SetProperty(ref this.uniqueId, value); }
        }

        private string title = string.Empty;

        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        private string description = string.Empty;

        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        private ImageSource image;
        private string imagePath;

        public ImageSource Image
        {
            get
            {
                if (this.image == null && this.imagePath != null)
                {
                    this.image = new BitmapImage(new Uri(baseUri, this.imagePath));
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