namespace Ni.Store.Api.Models.Responses
{
    public class StorePutResponse : CoreResponse
    {
        public int Id { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
