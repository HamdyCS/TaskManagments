using Application.Common.Interfaces.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Infrastructure.common.channels
{
    public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
    {
        private readonly Channel<T> _channel;
        public BackgroundQueue()
        {
            _channel = Channel.CreateUnbounded<T>();
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }

        public async Task EnqueueAsync(T item)
        {

            await _channel.Writer.WriteAsync(item);
        }
    }
}
