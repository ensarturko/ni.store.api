namespace Ni.Store.Api.Models.Responses
{
    public class StorePostResponse : CoreResponse
    {
        public int Id { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
