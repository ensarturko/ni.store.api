using System;

namespace Ni.Store.Api.Data.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
