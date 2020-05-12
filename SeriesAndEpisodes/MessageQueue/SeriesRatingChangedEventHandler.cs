using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.MessageQueue
{
    public class SeriesRatingChangedEventHandler : IConsumer<ISeriesRatingChangedEvent>
    {
        public Task Consume(ConsumeContext<ISeriesRatingChangedEvent> context)
        {
            Console.WriteLine($"Consumed (SeriesId: {context.Message.SeriesId}, Count: {context.Message.Count}, Average: {context.Message.Average})");
            return Task.CompletedTask;
        }
    }
}
