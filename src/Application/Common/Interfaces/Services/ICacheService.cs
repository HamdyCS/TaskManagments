using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key,bool throwOnFailure = false);
        Task SetAsync<T>(string key, T value,TimeSpan AbsoluteExpiration, bool throwOnFailure = false);
        Task RemoveAsync(string key, bool throwOnFailure = false);

    }
}
