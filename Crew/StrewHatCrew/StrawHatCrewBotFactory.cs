using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using StrewHatCrew.Bot;

namespace StrewHatCrew
{
    public class StrawHatCrewBotFactory(IConfiguration config, CancellationTokenSource? cancellationTokenSource)
    {
        private readonly IConfiguration _configuration = config;
        protected readonly Random _random = Random.Shared;

        public ICrewMemberBot CreateCrewMemberBot(string? characterName)
        {
            cancellationTokenSource ??= new CancellationTokenSource();
            return characterName?.ToLower() switch
            {
                StrawHatCrewBot.LUFFY => new LuffyBot(cancellationTokenSource),
                StrawHatCrewBot.ZORO => new ZoroBot(cancellationTokenSource),
                StrawHatCrewBot.NAMI => new NamiBot(cancellationTokenSource),
                StrawHatCrewBot.USOPP => new UsoppBot(cancellationTokenSource),
                StrawHatCrewBot.SANJI => new SanjiBot(cancellationTokenSource),
                StrawHatCrewBot.CHOPPER => new ChopperBot(cancellationTokenSource),
                StrawHatCrewBot.ROBIN => new RobinBot(cancellationTokenSource),
                StrawHatCrewBot.FRANKY => new FrankyBot(cancellationTokenSource),
                StrawHatCrewBot.BROOK => new BrookBot(cancellationTokenSource),
                _ => throw new ArgumentException($"Unknown character: {characterName}")
            };
        }

        public async Task<StrawHatCrewBot> CreateCrewBotAsync(string token, string? guildId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty");
            }

            DiscordSocketConfig clientConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
                AlwaysDownloadUsers = true
            };

            DiscordSocketClient client = new DiscordSocketClient(clientConfig);
            await using StrawHatCrewBot crewBot = new StrawHatCrewBot(client, guildId, _configuration);
            await crewBot.InitializeAsync(token, cancellationToken);
            return crewBot;
        }
    }
}