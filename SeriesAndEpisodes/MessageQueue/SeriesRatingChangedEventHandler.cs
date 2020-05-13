using MassTransit;
using SeriesAndEpisodes.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.MessageQueue
{
    public class SeriesRatingChangedEventHandler : IConsumer<ISeriesRatingChangedEvent>
    {

        readonly SeriesService _seriesService;

        public SeriesRatingChangedEventHandler(SeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        public async Task Consume(ConsumeContext<ISeriesRatingChangedEvent> context)
        {
            await _seriesService.UpdateRatingsAsync(context.Message.SeriesId, context.Message.Average, context.Message.Count);
            Console.WriteLine($"Consumed: {JsonSerializer.Serialize(context)}");
        }
    }
}
