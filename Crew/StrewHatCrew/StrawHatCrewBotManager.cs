using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace StrewHatCrew
{
    public class StrawHatCrewBotManager(IConfiguration config, CancellationTokenSource? cancellationTokenSource)
    {
        protected readonly Dictionary<string, StrawHatCrewBot> _bots = new Dictionary<string, StrawHatCrewBot>();
        protected readonly StrawHatCrewBotFactory _factory = new StrawHatCrewBotFactory(config, cancellationTokenSource);
        protected readonly IConfiguration _config = config;

        public async Task StartNewBotAsync(string guildId)
        {
            if (_bots.ContainsKey(guildId))
            {
                throw new InvalidOperationException($"Bot already exists for guild {guildId}");
            }

            string? token = _config[$"Tokens:{guildId}"] ?? _config["DefaultToken"];
            if(string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty");
            }
            StrawHatCrewBot? bot = await _factory.CreateCrewBotAsync(token, guildId);
            _bots.Add(guildId, bot);
        }

        public async Task StopBotAsync(string guildId, CancellationToken cancellationToken = default)
        {
            if (_bots.TryGetValue(guildId, out var bot))
            {
                await bot.StopAsync(cancellationToken);
                _bots.Remove(guildId);
            }
        }

        public async Task StopAllBotsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var bot in _bots.Values)
            {
                await bot.StopAsync(cancellationToken);
            }
            _bots.Clear();
        }

        public StrawHatCrewBot? GetBot(string guildId)
        {
            return _bots.TryGetValue(guildId, out StrawHatCrewBot? bot) ? bot : null;
        }
    }

    public partial class StrawHatCrewBot
    {
        public const string LUFFY = "luffy";
        public const string ZORO = "zoro";
        public const string NAMI = "nami";
        public const string USOPP = "usopp";
        public const string SANJI = "sanji";
        public const string CHOPPER = "chopper";
        public const string ROBIN = "robin";
        public const string FRANKY = "franky";
        public const string BROOK = "brook";

        protected readonly string? _guildId;
        protected readonly IConfiguration _configuration;

        public StrawHatCrewBot(DiscordSocketClient client, string? guildId, IConfiguration configuration, CancellationTokenSource? cancellationTokenSource = null)
        {
            _client = client;
            _guildId = guildId;
            _crewMembers = new Dictionary<string, ICrewMemberBot>();
            InitializeCrewMembers();
            _configuration = configuration;
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            _messageReceivedAsyncFunc = (socketMessage) => HandleMessageAsync(socketMessage, _cancellationTokenSource.Token);
            _logAsyncFunc = (logMessage) => LogAsync(logMessage, _cancellationTokenSource.Token);
            _client.MessageReceived += _messageReceivedAsyncFunc;
        }

        protected virtual void InitializeCrewMembers()
        {
            StrawHatCrewBotFactory? factory = new StrawHatCrewBotFactory(_configuration, _cancellationTokenSource);
            string[]? characters = [LUFFY, ZORO, NAMI, USOPP, SANJI, CHOPPER, ROBIN, FRANKY, BROOK];

            foreach (string? character in characters)
            {
                ICrewMemberBot bot = factory.CreateCrewMemberBot(character);
                _crewMembers.Add(bot.GetCommandPrefix(), bot);
            }
        }
    }

    // メインプログラム
    public partial class Program
    {
        public static async Task MainAsync(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            StrawHatCrewBotManager botManager = new StrawHatCrewBotManager(config, _cancellationTokenSource);

            // 設定ファイルから複数のギルドIDを読み込む
            List<string?>? guildIds = config.GetSection("GuildIds").Get<List<string?>>();

            foreach (string? guildId in guildIds ?? [])
            {
                if(string.IsNullOrWhiteSpace(guildId))
                {
                    Console.WriteLine("Invalid guild ID");
                    continue;
                }
                try
                {
                    await botManager.StartNewBotAsync(guildId);
                    Console.WriteLine($"Started bot for guild: {guildId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start bot for guild {guildId}: {ex.Message}");
                }
            }

            // Ctrl+C で終了
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Console.CancelKeyPress += async (s, e) =>
            {
                e.Cancel = true;
                await botManager.StopAllBotsAsync(_cancellationTokenSource.Token);
                tcs.SetResult(true);
            };

            try
            {
                await tcs.Task;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Bot shutdown complete.");
            }
        }
    }
}