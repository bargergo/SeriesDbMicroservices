namespace SeriesAndEpisodes.Models
{
    public class FileSettings : IFileSettings
    {
        public int MaxSizeInMegabytes { get; set; }
    }

    public interface IFileSettings
    {
        public int MaxSizeInMegabytes { get; }
    }
}
