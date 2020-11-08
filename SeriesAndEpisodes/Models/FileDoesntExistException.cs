using System;
using System.Runtime.Serialization;

namespace SeriesAndEpisodes.Controllers
{
    [Serializable]
    internal class FileDoesntExistException : Exception
    {
        public FileDoesntExistException()
        {
        }

        public FileDoesntExistException(string message) : base(message)
        {
        }

        public FileDoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FileDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}