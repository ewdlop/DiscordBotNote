using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using StrewHatCrew.Bot;

namespace StrewHatCrew
{
    public partial class StrawHatCrewBot : IDisposable, IAsyncDisposable
    {
        protected readonly Dictionary<string, ICrewMemberBot> _crewMembers;
        protected readonly DiscordSocketClient _client;
        protected readonly CancellationTokenSource _cancellationTokenSource;
        protected bool _disposed = false;
        protected readonly Func<LogMessage, Task> _logAsyncFunc;
        protected readonly Func<SocketMessage, Task> _messageReceivedAsyncFunc;

        public StrawHatCrewBot(IConfiguration configuration, CancellationTokenSource? cancellationTokenSource = default)
        {
            _configuration = configuration;
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            _crewMembers = new Dictionary<string, ICrewMemberBot>
            {
                { BuildCommandPrefix<LuffyBot>(), new LuffyBot(_cancellationTokenSource) },
                { BuildCommandPrefix<ZoroBot>(), new ZoroBot(_cancellationTokenSource) },
                { BuildCommandPrefix<NamiBot>(), new NamiBot(_cancellationTokenSource) },
                { BuildCommandPrefix<UsoppBot>(), new UsoppBot(_cancellationTokenSource) },
                { BuildCommandPrefix<SanjiBot>(), new SanjiBot(_cancellationTokenSource) },
                { BuildCommandPrefix<ChopperBot>(), new ChopperBot(_cancellationTokenSource) },
                { BuildCommandPrefix<RobinBot>(), new RobinBot(_cancellationTokenSource) },
                { BuildCommandPrefix<FrankyBot>(), new FrankyBot(_cancellationTokenSource) },
                { BuildCommandPrefix<BrookBot>(), new BrookBot(_cancellationTokenSource) }
            };

            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            _logAsyncFunc = (logMessage) => LogAsync(logMessage, _cancellationTokenSource.Token);
            _messageReceivedAsyncFunc = (socketMessage) => HandleMessageAsync(socketMessage, _cancellationTokenSource.Token);

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 1024,
            });
            _client.Log += _logAsyncFunc;
            _client.MessageReceived += _messageReceivedAsyncFunc;
        }

        protected virtual string BuildCommandPrefix<T>() => typeof(T).Name switch
        {
            nameof(LuffyBot) => PrependExclamationMark(LUFFY),
            nameof(ZoroBot) => PrependExclamationMark(ZORO),
            nameof(NamiBot) => PrependExclamationMark(NAMI),
            nameof(UsoppBot) => PrependExclamationMark(USOPP),
            nameof(SanjiBot) => PrependExclamationMark(SANJI),
            nameof(ChopperBot) => PrependExclamationMark(CHOPPER),
            nameof(RobinBot) => PrependExclamationMark(ROBIN),
            nameof(FrankyBot) => PrependExclamationMark(FRANKY),
            nameof(BrookBot) => PrependExclamationMark(BROOK),
            _ => throw new ArgumentException($"Unknown bot: {typeof(T).Name}")
        };

        public virtual string PrependExclamationMark(string value)
        {
            return $"!{value}";
        }

        protected virtual Task HandleMessageAsync(SocketMessage message, CancellationToken cancellationToken = default)
        {
            if (message.Author.Id == _client.CurrentUser.Id)
                return Task.CompletedTask;

            string[] parts = message.Content.Split(' ');
            if (parts.Length == 0)
                return Task.CompletedTask;

            string prefix = parts[0].ToLower();
            if (_crewMembers.TryGetValue(prefix, out ICrewMemberBot? crewMember))
            {
                string command = parts.Length > 1 ? parts[1].ToLower() : "help";
                return Task.Run(async ()=>await crewMember.HandleCommandAsync(message, command), cancellationToken);
            }

            return Task.CompletedTask;
        }


        public virtual async Task StartAsync(string? token, CancellationToken cancellationToken = default)
        {
            _client.Log += _logAsyncFunc;
            await Task.Run(() => _client.LoginAsync(TokenType.Bot, token), cancellationToken);
            await Task.Run(() => _client.StartAsync(), cancellationToken);

            // ボットのステータスを設定
            await Task.Run(async () =>
            {
                await _client.SetGameAsync("麦わらの一味と冒険中！");
            }, cancellationToken);
        }

        public async Task InitializeAsync(string token, CancellationToken cancellationToken = default)
        {
            await Task.Run(async () => await _client.LoginAsync(TokenType.Bot, token), cancellationToken);
            await Task.Run(_client.StartAsync, cancellationToken);
            await Task.Run(()=>_client.SetGameAsync($"Guild: {_guildId} の麦わらの一味！"), cancellationToken);
        }

        protected virtual Task LogAsync(LogMessage log, CancellationToken cancellationToken)
        {
            return Task.Run(() => Console.WriteLine(log.ToString()), cancellationToken);
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(_client.LogoutAsync, cancellationToken);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!_disposed)
            {
                if (dispose)
                {
                    _client.Log -= _logAsyncFunc;
                    _client.MessageReceived -= _messageReceivedAsyncFunc;
                    _client.Dispose();
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
                    _client.Log -= _logAsyncFunc;
                    _client.MessageReceived -= _messageReceivedAsyncFunc;
                    _client.Dispose();
                    await Task.Run(_client.DisposeAsync, _cancellationTokenSource.Token);
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
    }
}