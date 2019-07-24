namespace Ni.Store.Api.Models.Responses
{
    public class StoreGetResponse
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}