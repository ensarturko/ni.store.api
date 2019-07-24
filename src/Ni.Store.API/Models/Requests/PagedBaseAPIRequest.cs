namespace Ni.Store.Api.Models.Requests
{
    public class PagedBaseApiRequest
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}