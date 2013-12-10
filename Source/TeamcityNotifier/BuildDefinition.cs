﻿namespace TeamcityNotifier
{
    using System;
    using System.Collections.Generic;

    using DataAbstraction;

    public class BuildDefinition : IBuildDefinition
    {
        private readonly string url;

        public BuildDefinition(string url)
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
                return typeof(buildType);
            }
        }

        public IEnumerable<IRestObject> Dependencies
        {
            get
            {
                return new List<IRestObject>();
            }
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string BuildRepositoryUrl { get; private set; }

        public void SetData(object obj)
        {
            var baseObject = (buildType) obj;

            this.Id = baseObject.id;
            this.Name = baseObject.name;
            this.Description = baseObject.description;
            this.BuildRepositoryUrl = baseObject.builds.href;
        }
    }
}