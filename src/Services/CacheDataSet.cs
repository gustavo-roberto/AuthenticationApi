using System;

namespace AuthenticationApi.Services
{
    public class CacheDataSet
    {
        public DateTimeOffset ExpiresAt { get; set; }
        public int attempts { get; set; }
    }
}