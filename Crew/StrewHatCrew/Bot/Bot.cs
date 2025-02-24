using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public abstract class Bot(CancellationTokenSource? cancellationTokenSource) : ICrewMemberBot
    {
        protected readonly CancellationTokenSource _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
        protected bool _disposed = false;
        protected readonly Random _random = Random.Shared;
        protected virtual void Dispose(bool dispose)
        {
            if (!_disposed)
            {
                if (dispose)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                Interlocked.Exchange(ref _disposed, true);
            }
        }

        protected async virtual ValueTask DisposeAsync(bool dispose)
        {
            if (!_disposed)
            {
                if (dispose)
                {
                    await Task.Run(_cancellationTokenSource.CancelAsync, _cancellationTokenSource.Token);
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                Interlocked.Exchange(ref _disposed, true);
            }
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return DisposeAsync(true);
        }

        public abstract Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default);
        public abstract string GetCharacterName();
        public abstract string GetCommandPrefix();
    }
}