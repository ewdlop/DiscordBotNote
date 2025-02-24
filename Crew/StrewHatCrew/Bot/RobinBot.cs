using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class RobinBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "ロビン";
        public override string GetCommandPrefix() => "!robin";

        protected readonly string[] _history = {
            "空白の100年には多くの謎が隠されています。",
            "ポーネグリフには古代兵器の情報が刻まれています。",
            "歴史は繰り返されるものです。"
        };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "history":
                    string fact = _history[_random.Next(_history.Length)];
                    return Task.Run(() => message.Channel.SendMessageAsync(fact), cancellationToken);
                case "hana":
                    return Task.Run(() => message.Channel.SendMessageAsync("花花の実の能力、トレンタ・フルール！"), cancellationToken);
                case "read":
                    return Task.Run(() => message.Channel.SendMessageAsync("この文字は古代文字ね。解読してみましょう。"), cancellationToken);
                case "help":
                    return Task.Run(() => message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!robin history - 歴史の話\n" +
                        "!robin hana - 花花の実の能力\n" +
                        "!robin read - 古代文字の解読"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}