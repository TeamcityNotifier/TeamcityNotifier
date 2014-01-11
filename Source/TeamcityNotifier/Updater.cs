namespace TeamcityNotifier
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Updater : IUpdater
    {
        private const int TimeIntervalInMilliseconds = 10000;

        private readonly List<IRestObject> restObjectsToUpdate;

        private readonly IRestConsumer restConsumer;

        private volatile CancellationTokenSource cancellationTokenSource;

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
            this.cancellationTokenSource = new CancellationTokenSource();
            
            Task.Delay(TimeIntervalInMilliseconds, cancellationTokenSource.Token).ContinueWith(this.Tick);
        }

        public void Stop()
        {
            this.cancellationTokenSource.Cancel();
        }

        private void Tick(Task task)
        {
            if (task.Status == TaskStatus.RanToCompletion)
            {
                foreach (var restObject in this.restObjectsToUpdate)
                {
                    this.restConsumer.Load(restObject);
                }

                Task.Delay(1000, this.cancellationTokenSource.Token).ContinueWith(Tick);
            }
        }
    }
}