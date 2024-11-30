namespace Students.Models
{
    public class JwtSettings
    {
        public string Secret;
        public string Issuer;
        public string Audience;
        public int ExpiryInMinutes {  get; set; }
    }
}
