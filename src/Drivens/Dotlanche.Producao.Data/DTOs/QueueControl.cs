using MongoDB.Bson.Serialization.Attributes;

namespace Dotlanche.Producao.Data.DTOs
{
    internal class QueueControl
    {
        private const int InitialKey = 1;

        public QueueControl(int queueKey)
        {
            QueueKey = queueKey;
            GenerationTime = DateTime.Now;
        }

        [BsonId]
        public int QueueKey { get; private set; }

        public DateTime GenerationTime { get; private set; }

        public static QueueControl GetInitialQueueKey() => new(InitialKey);
    }
}