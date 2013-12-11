namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    using DataAbstraction;

    internal class Build : IBuild
    {
        private readonly string url;

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

        public long Id { get; set; }

        public string Status { get; private set; }

        public string Number { get; private set; }

        public string FinishDate { get; set; }

        public string StartDate { get; set; }

        public void SetData(object obj)
        {
            var baseObject = (build)obj;

            this.Id = baseObject.id;
            this.Number = baseObject.number;
            this.Status = baseObject.status;
            this.StartDate = baseObject.startDate;
            this.FinishDate = baseObject.finishDate;
        }
    }
}