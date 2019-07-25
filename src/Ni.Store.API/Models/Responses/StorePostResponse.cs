using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ni.Store.Api.Models.Responses
{
    public class StorePostResponse
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
