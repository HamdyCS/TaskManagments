using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Channels
{
    public interface IBackgroundQueue<T> where T : class
    {
        Task EnqueueAsync(T item);
        Task<T> DequeueAsync(CancellationToken cancellationToken);
    }

}
