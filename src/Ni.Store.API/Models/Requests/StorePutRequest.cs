using System;

namespace Ni.Store.Api.Models.Requests
{
    public class StorePutRequest : CoreRequest
    {
        public DateTime? ExpirationTime { get; set; }
    }
}
