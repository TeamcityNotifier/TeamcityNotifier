namespace TeamcityNotifier
{
    using System.Collections.Generic;

    public class Updater : IUpdater
    {
        private readonly List<IRestObject> restObjectsToUpdate;

        private readonly IRestConsumer restConsumer;

        public Updater(IRestConsumer restConsumer)
        {
            this.restConsumer = restConsumer;
            this.restObjectsToUpdate = new List<IRestObject>();
        }

        public void Register(IRestObject restObject)
        {
            this.restObjectsToUpdate.Add(restObject);
        }

        public void Start()
        {
            this.Tick();
        }

        public void Tick()
        {
            foreach (var restObject in this.restObjectsToUpdate)
            {
                this.restConsumer.Load(restObject);
            }
        }
    }
}