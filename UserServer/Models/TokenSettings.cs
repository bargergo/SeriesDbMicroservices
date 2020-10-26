namespace UserServer.Models
{
    public interface ITokenSettings
    {
        public string Issuer { get; }
        public string Audience { get; }
        public string Secret { get; }
    }
    public class TokenSettings : ITokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
    }
}
