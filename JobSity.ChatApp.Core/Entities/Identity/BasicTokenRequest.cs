namespace JobSity.ChatApp.Core.Entities.Identity
{
    public class BasicTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Address { get; set; }
        public string Scope { get; set; }
    }
}