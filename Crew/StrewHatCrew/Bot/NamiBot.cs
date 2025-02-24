using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class NamiBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "ナミ";
        public override string GetCommandPrefix() => "!nami";

        protected readonly string[] _weather = { "晴れ", "雨", "曇り", "強風", "雷雨" };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "weather":
                    string forecast = _weather[_random.Next(_weather.Length)];
                    return Task.Run(()=> message.Channel.SendMessageAsync($"今日の天気は{forecast}になりそうね。"), cancellationToken);
                case "money":
                    return Task.Run(()=> message.Channel.SendMessageAsync("借金は必ず返してもらうわよ！利子20%つけて！"), cancellationToken);
                case "climatact":
                    return Task.Run(()=> message.Channel.SendMessageAsync("天候棒で嵐を起こすわ！"), cancellationToken);
                case "help":
                    return Task.Run(()=> message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!nami weather - 天気予報\n" +
                        "!nami money - お金の話\n" +
                        "!nami climatact - 天候棒の使用"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}