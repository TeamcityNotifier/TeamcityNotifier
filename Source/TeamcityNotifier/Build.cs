namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DataAbstraction;
    
    internal class Build : IBuild
    {
        private readonly string url;

        private long id;

        private Status status;

        private string number;

        private string finishDate;

        private string startDate;

        private string webUrl;

        public event PropertyChangedEventHandler PropertyChanged;

        public Build(string url)
        {
            this.url = url;
        }

        public string Url
        {
            get
            {
                return this.url;
            }
        }

        public Type BaseType
        {
            get
            {
                return typeof(build);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return new List<IRestObject>();
            }
        }

        public long Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id == value)
                {
                    return;
                }

                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public string WebUrl
        {
            get
            {
                return this.webUrl;
            }
            set
            {
                if (this.webUrl == value)
                {
                    return;
                }

                this.webUrl = value;
                this.OnPropertyChanged("WebUrl");
            }
        }

        public Status Status
        {
            get
            {
                return this.status;
            }
            private set
            {
                if (this.status == value)
                {
                    return;
                }

                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public string Number
        {
            get
            {
                return this.number;
            }
            private set
            {
                if (this.number == value)
                {
                    return;
                }

                this.number = value;
                this.OnPropertyChanged("Number");
            }
        }

        public string FinishDate
        {
            get
            {
                return this.finishDate;
            }
            set
            {
                if (this.finishDate == value)
                {
                    return;
                }

                this.finishDate = value;
                this.OnPropertyChanged("FinishDate");
            }
        }

        public string StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                if (this.startDate == value)
                {
                    return;
                }

                this.startDate = value;
                this.OnPropertyChanged("StartDate");
            }
        }

        public void SetData(object obj)
        {
            var baseObject = (build)obj;

            this.Id = baseObject.id;
            this.WebUrl = baseObject.webUrl;
            this.Status = EnumParser.GetStatusFor(baseObject.status);
            this.Number = baseObject.number;
            this.StartDate = baseObject.startDate;
            this.FinishDate = baseObject.finishDate;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}