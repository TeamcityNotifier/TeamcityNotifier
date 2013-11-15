namespace TeamcityNotifier
{
    using DataAbstraction;

    public class BuildDefinition : IBuildDefinition
    {
        private readonly buildType buildType;

        public BuildDefinition(buildType buildType)
        {
            this.buildType = buildType;
        }


        public string Name
        {
            get
            {
                return this.buildType.name;
            }
        }

        public string Description
        {
            get
            {
                return this.buildType.description;
            }
        }

        public string Href
        {
            get
            {
                return this.buildType.href;
            }
        }
    }
}