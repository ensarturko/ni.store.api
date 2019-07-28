using System;

namespace Ni.Store.Api.Models.Responses
{
    public class StorePostResponse : CoreResponse
    {
        public int Id { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
